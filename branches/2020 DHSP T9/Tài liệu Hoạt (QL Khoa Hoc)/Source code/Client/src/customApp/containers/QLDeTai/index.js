import React, { Component } from "react";
import api from "./config";
import { connect } from "react-redux";
import { Link } from "react-router-dom";
import * as actions from "../../redux/QLDeTai/actions";
import LayoutWrapper from "../../../components/utility/layoutWrapper";
import PageHeader from "../../../components/utility/pageHeader";
import PageAction from "../../../components/utility/pageAction";
import { changeUrlFilter, getDefaultPageSize, getFilterData, getOptionSidebar } from "../../../helpers/utility";
import queryString from "query-string";
import Box from "../../../components/utility/box";
import BoxFilter from "../../../components/utility/boxFilter";
import BoxTable, { EmptyTable } from "../../../components/utility/boxTable";
import { message, Input, Icon, Button, Select, Modal } from "antd";
import ThemDeTaiModal from "./ThemDeTaiModal";
import ExcelComponent from "./excelTable";
// import GuiDeTaiModal from "./GuiDeTaiModal";
import { getRoleByKey2 } from "../../../helpers/utility";
import { ValidatorForm } from "react-form-validator-core";
import { TreeSelectWithApi, SelectWithApi, GoTruncate } from "../../components/index";
import { isArray } from "lodash";
import moment from "moment";

class QLDeTai extends Component {
  constructor(props) {
    super(props);
    document.title = "Quản lý nhiệm vụ nghiên cứu";
    const filterData = queryString.parse(this.props.location.search);
    this.state = {
      filterData: filterData,
      selectedRowKeys: [],
      excelData: null,
    };
  }
  modal = null;
  //Get initData---------------------------------------------
  componentDidMount = () => {
    this.props.getList(this.state.filterData);
  };
  //filter --------------------------------------------------
  onFilter = (value, property) => {
    //get filter data
    let oldFilterData = this.state.filterData;
    let onFilter = { value, property };
    let filterData = getFilterData(oldFilterData, onFilter, null);
    //get filter data
    this.setState(
      {
        filterData,
        selectedRowKeys: [],
      },
      () => {
        changeUrlFilter(this.state.filterData); //change url
        this.props.getList(this.state.filterData); //get list
      }
    );
  };
  handleChangeKeyword = (event) => {
    if (this.timeout) clearTimeout(this.timeout);
    const { filterData } = this.state;
    filterData.Keyword = event.target.value;
    this.setState({ filterData });
    // if (!event.target.value) {
    //   this.onFilter();
    // }
    this.timeout = setTimeout(() => {
      this.onFilter(filterData.Keyword, "Keyword");
    }, 1000);
  };
  //order, paging --------------------------------------------
  onTableChange = (pagination, filters, sorter) => {
    //get filter data
    let oldFilterData = this.state.filterData;
    let onOrder = { pagination, filters, sorter };
    let filterData = getFilterData(oldFilterData, null, onOrder);
    //get filter data
    this.setState(
      {
        filterData,
        selectedRowKeys: [],
      },
      () => {
        changeUrlFilter(this.state.filterData); //change url
        this.props.getList(this.state.filterData); //get list
      }
    );
  };

  handleCloseModal = () => {
    this.modal.destroy();
    this.props.getList(this.state.filterData);
  };
  handleDelete = (record) => {
    Modal.confirm({
      title: "Xoá nhiệm vụ nghiên cứu này",
      content: "Bạn có chắc chắn muốn xoá nhiệm vụ nghiên cứu này hay không?",
      okText: "Xoá",
      cancelText: "Huỷ",
      onOk: () => {
        api
          .xoaDeTai({
            DeTaiID: record.DeTaiID,
          })
          .then((res) => {
            if (!res || !res.data || res.data.Status !== 1) {
              this.setState({ confirmLoading: false });
              message.error(`${res && res.data ? res.data.Message : "Lỗi hệ thống"}`);
            } else {
              message.success("Xoá nhiệm vụ nghiên cứu thành công");
              this.props.getList(this.state.filterData);
              this.state.selectedRowKeys = [];
            }
          });
      },
    });
  };

  handleOpenModal = (name) => () => {
    switch (name) {
      case "ThemDeTaiModal":
        this.modal = Modal.confirm({
          icon: <i />,
          content: <ThemDeTaiModal data={null} onClose={this.handleCloseModal}></ThemDeTaiModal>,
        });
        break;

      default:
        break;
    }
  };
  handleExportExcel = () => {
    const { TotalRow } = this.props;
    const hide = message.loading("Đang xuất dữ liệu..", 0);

    api
      .danhSachDeTai({ PageSize: TotalRow })
      .then((res) => {
        if (res.data && res.data.Status === 1) {
          this.setState({ excelData: res.data.Data });
          setTimeout(hide, 1000);
        } else {
          message.error(`${res.data && res.data.Message ? res.data.Message : "Xuất excel thất bại"}`);
          setTimeout(hide, 0);
        }
      })
      .catch((err) => {
        console.log(err);
        setTimeout(hide, 0);
        message.error(`Xuất excel thất bại`);
      });
  };

  render() {
    const { listDeTai, TotalRow, role } = this.props;
    const { filterData } = this.state;
    const PageNumber = filterData.PageNumber ? parseInt(filterData.PageNumber) : 1;
    const PageSize = filterData.PageSize ? parseInt(filterData.PageSize) : getDefaultPageSize();
    const tableEle = document.getElementById("table");

    const columns = [
      {
        title: "STT",
        align: "center",
        width: 50,
        render: (text, record, index) => <span>{(PageNumber - 1) * PageSize + (index + 1)}</span>,
      },
      {
        title: "Mã nhiệm vụ",
        dataIndex: "MaDeTai",
        key: "MaDeTai",
        width: 100,
      },
      {
        title: "Tên nhiệm vụ",
        dataIndex: "TenDeTai",
        key: "TenDeTai",
        width: 200,
        render: (text, record, index) => (
          <Link
            className={`${moment(record.NamKetThuc, "MM/YYYY").endOf("month").isBefore(moment().endOf("month")) && record.TrangThai !== 2 && record.TrangThai !== 3 ? "text-danger" : ""}`}
            to={`ql-de-tai/chi-tiet/${record.DeTaiID}`}
          >
            <p className="mb-0">{text}</p>
          </Link>
        ),
      },
      {
        title: "Chủ nhiệm đề tài",
        dataIndex: "TenChuNhiemDeTai",
        key: "TenChuNhiemDeTai",
        width: 200,
      },
      {
        title: "Loại hình nghiên cứu",
        dataIndex: "TenLoaiHinhNghienCuu",
        key: "TenLoaiHinhNghienCuu",
        width: 200,
      },
      {
        title: "Lĩnh vực nghiên cứu",
        dataIndex: "TenLinhVucNghienCuu",
        key: "TenLinhVucNghienCuu",
        width: 200,
      },
      {
        title: "Cấp quản lý/Loại nhiệm vụ",
        dataIndex: "TenCapQuanLy",
        key: "TenCapQuanLy  ",
        width: 200,
      },
      {
        title: "Năm bắt đầu",
        dataIndex: "NamBatDau",
        key: "NamBatDau",
        width: 100,
      },
      {
        title: "Năm kết thúc",
        dataIndex: "NamKetThuc",
        key: "NamKetThuc",
        width: 100,
      },
      {
        title: "Trạng thái",
        dataIndex: "TrangThai",
        key: "TrangThai",
        width: 100,
        render: (text, record, index) => <div>{record.TrangThai === 1 ? "Đang thực hiện" : record.TrangThai === 2 ? "Nghiệm thu" : "Thanh lý"}</div>,
      },
      {
        title: "Thành viên tham gia",
        dataIndex: "ThanhVienNghienCuuStr",
        key: "ThanhVienNghienCuuStr",
        width: 200,
      },
      {
        title: "Mục tiêu",
        dataIndex: "MucTieu",
        key: "Muctieu",
        width: 300,
        render: (text, record, index) => (
          <div>
            <GoTruncate value={text}></GoTruncate>
          </div>
        ),
      },
      {
        title: "Các nội dung chính",
        dataIndex: "CacNoiDungChinhStr",
        key: "CacNoiDungChinhStr",
        width: 300,
      },
      {
        title: "Sản phẩm đăng ký",
        dataIndex: "SanPhamDangKy",
        key: "SanPhamDangKy",
        width: 300,
        render: (text, record, index) => (
          <div>
            <GoTruncate value={text}></GoTruncate>
          </div>
        ),
      },
      {
        title: "Khả năng ứng dụng của đề tài",
        dataIndex: "KhaNangUngDung",
        key: "KhaNangUngDung",
        width: 300,
        render: (text, record, index) => (
          <div>
            <GoTruncate value={text}></GoTruncate>
          </div>
        ),
      },
      {
        title: "Hành động",
        dataIndex: "Hanhdong",
        key: "HanhDong",
        width: 200,
        render: (text, record) => {
          return (
            <div className="text-center">
              {role.delete ? (
                <div className="d-inline-block px-1">
                  <Button
                    type="primary"
                    onClick={() => {
                      this.handleDelete(record);
                    }}
                  >
                    Xoá
                  </Button>
                </div>
              ) : (
                ""
              )}
            </div>
          );
        },
      },
    ];
    return (
      <LayoutWrapper>
        <PageHeader>Quản lý nhiệm vụ nghiên cứu</PageHeader>
        <PageAction>
          <div>
            <div className="d-inline-block">
              {role.view ? (
                <Button type="primary" onClick={this.handleExportExcel}>
                  Xuất Excel
                </Button>
              ) : (
                ""
              )}
              {/* {role.delete ? (
                <div className="d-inline-block">
                  <Button type="primary" onClick={this.handleDelete} disabled={this.state.selectedRowKeys.length !== 1 || this.state.selectedRowKeys.length === 0}>
                    Xoá
                  </Button>
                </div>
              ) : (
                ""
              )} */}
              {role.add ? (
                <Button type="primary" onClick={this.handleOpenModal("ThemDeTaiModal")}>
                  Thêm nhiệm vụ
                </Button>
              ) : (
                ""
              )}
            </div>
          </div>
        </PageAction>
        <Box>
          <ValidatorForm onSubmit={() => {}}>
            <div className="row mb-2 mr-1">
              <div className="col-6 col-lg-2">
                <TreeSelectWithApi
                  apiConfig={{
                    api: api.danhSachCayLinhVuc,
                    valueField: "ID",
                    nameField: "Name",
                    codeField: "Code",
                    filter: {
                      // Type: 1,
                      status: true,
                    },
                  }}
                  placeholder="Chọn lĩnh vực"
                  onChange={(value) => {
                    this.onFilter(value, "LinhVucNghienCuu");
                  }}
                  value={filterData.LinhVucNghienCuu}
                />
              </div>
              <div className="col-6 col-lg-2">
                <TreeSelectWithApi
                  apiConfig={{
                    api: api.danhSachCapDeTai,
                    valueField: "ID",
                    nameField: "Name",
                  }}
                  placeholder="Cấp quản lý"
                  onChange={(value) => {
                    this.onFilter(value, "CapQuanLy");
                  }}
                  value={filterData.CapQuanLy}
                />
              </div>
              {role.qlDonVi || role.qlToanTruong ? (
                <div className="col-12 col-lg-2">
                  <SelectWithApi
                    apiConfig={{
                      api: api.danhSachCanBoTrongTruong,
                      valueField: "CanBoID",
                      nameField: "TenCanBo",
                      filter: {
                        PageSize: 2000,
                        departmentid: role.qlToanTruong ? 0 : JSON.parse(localStorage.getItem("user")).CoQuanID,
                      },
                    }}
                    useSearchAPI
                    // localSearch
                    placeholder="Chọn cán bộ"
                    value={filterData.CanBoID}
                    onChange={(value) => {
                      this.onFilter(value, "CanBoID");
                    }}
                    validators={["required"]}
                    errorMessages={["Nội dung bắt buộc"]}
                  ></SelectWithApi>
                </div>
              ) : null}

              <div className="col-12 col-lg-4">
                <Input
                  value={filterData.Keyword}
                  onChange={this.handleChangeKeyword}
                  onPressEnter={(e) => {
                    this.onFilter(e.target.value, "Keyword");
                  }}
                  placeholder="Nhập mã hoặc tên đề tài cần tìm kiếm"
                  allowClear
                  suffix={<Icon type="search" style={{ color: "rgba(0,0,0,.45)" }} />}
                ></Input>
              </div>
            </div>
          </ValidatorForm>
          <BoxTable
            id="table"
            columns={columns}
            rowKey="DeTaiID"
            rowClassName={(record, index) => {
              if (moment(record.NamKetThuc, "MM/YYYY").endOf("month").isBefore(moment().endOf("month")) && record.TrangThai !== 2 && record.TrangThai !== 3) {
                return "out-date";
              }
              return "";
            }}
            dataSource={listDeTai}
            loading={this.props.loading}
            onChange={this.onTableChange}
            pagination={{
              showSizeChanger: true, //show text: PageSize/page
              showTotal: (total, range) => `Từ ${range[0]} đến ${range[1]} trên ${total} kết quả`,
              total: TotalRow,
              current: PageNumber, //current page
              pageSize: PageSize,
            }}
          />
        </Box>
        {/* EXCEL EXPORT */}
        {this.state.excelData ? (
          <ExcelComponent
            callBack={() => {
              this.setState({ excelData: null });
            }}
            data={this.state.excelData}
          ></ExcelComponent>
        ) : (
          ""
        )}
      </LayoutWrapper>
    );
  }
}
function mapStateToProps(state) {
  const role = getRoleByKey2("ql-de-tai");
  const role4 = getRoleByKey2("ql-toan-truong");
  const role5 = getRoleByKey2("ql-don-vi");

  state.QLDeTai.role = { ...role, qlToanTruong: role4.view, qlDonVi: role5.view };
  // state.QLDeTai.role = { view: 1, edit: 1, add: 1, ...{ qlToanTruong: 1, qlDonVi: 1 } };
  return {
    ...state.QLDeTai,
  };
}
export default connect(mapStateToProps, actions)(QLDeTai);
