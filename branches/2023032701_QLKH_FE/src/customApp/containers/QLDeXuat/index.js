import React, { Component } from "react";
import { Link } from "react-router-dom";
import api, { TrangThai } from "./config";
import { connect } from "react-redux";
import * as actions from "../../redux/QLDeXuat/actions";
import LayoutWrapper from "../../../components/utility/layoutWrapper";
import PageHeader from "../../../components/utility/pageHeader";
import PageAction from "../../../components/utility/pageAction";
import { changeUrlFilter, getDefaultPageSize, getFilterData, getOptionSidebar } from "../../../helpers/utility";
import queryString from "query-string";
import Box from "../../../components/utility/box";
import BoxFilter from "../../../components/utility/boxFilter";
import BoxTable, { EmptyTable } from "../../../components/utility/boxTable";
import { Modal } from "antd";
import DeXuatModal from "./DeXuatModal";
import ExcelComponent from "./excelTable";
import DuyetDeXuatModal from "./DuyetDeXuatModal";
import { ValidatorForm } from "react-form-validator-core";
import { message, Input, Icon, Button, Select, Tooltip } from "antd";
import { Typography } from "antd4";
import { withAPI, GoSelect, GoTreeSelect } from "../../components/index";
import { getRoleByKey2 } from "../../../helpers/utility";
// import  Truncate from "react-truncate-html";
import { GoTruncate } from "../../components/index";
import numeral from "numeral";
import moment from "moment";
const SelectWithApi = withAPI(GoSelect);
const TreeSelectWithApi = withAPI(GoTreeSelect);
let timeout = null;
let modal = null;
class QLDeXuat extends Component {
  constructor(props) {
    super(props);
    document.title = "Đề xuất đề tài";
    const filterData = queryString.parse(this.props.location.search);
    this.state = {
      loading: false,

      filterData: filterData,
      selectedRowKeys: [],
      excelData: null,
      danhSachLinhVucNghienCuu: [],
      danhSachLinhVucKinhTe: [],
      danhSachCapQuanLy: [],
      danhSachTrangThai: [],
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
    const { filterData } = this.state;
    filterData.Keyword = event.target.value;
    this.setState({ filterData });
    if (!event.target.value) {
      this.onFilter();
    }
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
    this.state.selectedRowKeys = [];
    this.props.getList(this.state.filterData);
  };
  handleDelete = (record) => {
    Modal.confirm({
      title: "Xoá đề xuất",
      content: "Bạn có chắc chắn muốn xoá đề xuất đề tài này hay không?",
      okText: "Xoá",
      cancelText: "Huỷ",
      onOk: () => {
        api
          .xoaDeXuat({
            DeXuatID: record.DeXuatID,
          })
          .then((res) => {
            if (!res || !res.data || res.data.Status !== 1) {
              this.setState({ confirmLoading: false });
              message.error(`${res && res.data ? res.data.Message : "Lỗi hệ thống"}`);
            } else {
              message.success("Xoá đề xuất đề tài thành công");
              this.props.getList(this.state.filterData);
            }
          });
      },
    });
  };
  handleGuiDeXuat = (record) => {
    Modal.confirm({
      title: "Gửi đề xuất",
      content: "Bạn có chắc chắn muốn gửi đề xuất đề tài này hay không?",
      okText: "Gửi",
      cancelText: "Huỷ",
      onOk: () => {
        api
          .capNhatTrangThaiDeXuat({
            TrangThai: 2,
            NoiDung: "",
            DeXuatID: record.DeXuatID,
          })
          .then((res) => {
            if (!res || !res.data || res.data.Status !== 1) {
              this.setState({ confirmLoading: false });
              message.error(`${res && res.data ? res.data.Message : "Lỗi hệ thống"}`);
            } else {
              message.success("Gửi đề xuất thành công");
              this.props.getList(this.state.filterData);
            }
          });
      },
    });
  };
  handleExportExcel = (command) => {
    const { TotalRow } = this.props;
    const hide = message.loading("Đang xuất dữ liệu..", 0);

    api
      .danhSachDeXuat({ ...this.state.filterData, PageSize: TotalRow })
      .then((res) => {
        if (res.data && res.data.Status === 1) {
          this.setState({ excelData: res.data.Data, command: command });
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
  handleOpenModal = (name) => (record) => {
    this.modal = null;
    switch (name) {
      case "DeXuatModal":
        this.modal = Modal.confirm({
          icon: <i />,
          content: <DeXuatModal data={null} onClose={this.handleCloseModal}></DeXuatModal>,
        });
        break;

      case "DuyetDeXuatModal":
        this.modal = Modal.confirm({
          icon: <i />,
          content: <DuyetDeXuatModal dexuat={record} onClose={this.handleCloseModal}></DuyetDeXuatModal>,
        });
        break;

      default:
        break;
    }
  };

  render() {
    const { listDeXuat, TotalRow, role } = this.props;

    const { filterData } = this.state;
    const PageNumber = filterData.PageNumber ? parseInt(filterData.PageNumber) : 1;
    const PageSize = filterData.PageSize ? parseInt(filterData.PageSize) : getDefaultPageSize();
    const columns = [
      {
        title: "STT",
        align: "center",
        width: 50,
        // fixed: "left",
        render: (text, record, index) => <span>{(PageNumber - 1) * PageSize + (index + 1)}</span>,
      },
      {
        title: "Ngày đề xuất",
        dataIndex: "NgayDeXuat",
        key: "NgayDeXuat",
        width: 150,
        render: (text, record, index) => <p className="mb-0">{text ? moment(text).format("DD/MM/YYYY") : text}</p>,
      },
      {
        title: "Mã đề xuất",
        dataIndex: "MaDeXuat",
        key: "MaDeXuat",
        // fixed: "left",
        width: 100,
      },
      {
        title: "Tên đề xuất",
        dataIndex: "TenDeXuat",
        key: "TenDeXuat",
        // fixed: "left",
        width: 200,
        render: (text, record, index) => (
          <Link to={`ql-de-xuat/chi-tiet/${record.DeXuatID}`}>
            <p className="mb-0">{text}</p>
          </Link>
        ),
      },
      {
        title: "Lĩnh vực nghiên cứu",
        dataIndex: "TenLinhVucNghienCuu",
        key: "TenLinhVucNghienCuu",
        width: 200,
      },
      {
        title: "Lĩnh vực kinh tế - xã hội",
        dataIndex: "TenLinhVucKinhTeXaHoi",
        key: "TenLinhVucKinhTeXaHoi",
        width: 200,
      },
      {
        title: "Cấp quản lý",
        dataIndex: "TenCapQuanLy",
        key: "TenCapQuanLy",
        width: 200,
      },
      {
        title: "Nguời đề xuất",
        dataIndex: "TenNguoiDeXuat",
        key: "TenNguoiDeXuat",
        width: 200,
      },
      {
        title: "Tính cấp thiết",
        dataIndex: "TinhCapThiet",
        key: "TinhCapThiet",
        width: 300,

        render: (text, record, index) => (
          <div>
            <GoTruncate value={text}></GoTruncate>
          </div>
        ),
      },
      {
        title: "Mục tiêu",
        dataIndex: "MucTieu",
        key: "MucTieu",
        width: 300,

        render: (text, record, index) => (
          <div>
            <GoTruncate value={text}></GoTruncate>
          </div>
        ),
      },
      {
        title: "Nội dung",
        dataIndex: "NoiDung",
        key: "NoiDung",
        width: 300,
        ellipsis: true,
      },
      {
        title: "Sản phẩm",
        dataIndex: "SanPham",
        key: "SanPham",
        width: 300,

        render: (text, record, index) => (
          <div>
            <GoTruncate value={text}></GoTruncate>
          </div>
        ),
      },
      {
        title: "Kinh phí dự kiến",
        dataIndex: "KinhPhiDuKien",
        key: "KinhPhiDuKien",
        width: 100,
        render: (text, record, index) => <p className="mb-0">{record.KinhPhiDuKien ? numeral(record.KinhPhiDuKien).format("0,0") : ""}</p>,
      },
      {
        title: "Thời gian nghiên cứu",
        dataIndex: "ThoiGianNghienCuu",
        key: "ThoiGianNghienCuu",
        width: 100,
      },

      {
        title: "Thời gian thực hiện",
        dataIndex: "ThoiGianThucHienTu",
        key: "ThoiGianThucHienTu",
        width: 200,

        render: (text, record, index) => (
          <p className="mb-0">
            {record.ThoiGianThucHienTu} {record.ThoiGianThucHienDen ? "đến" : ""} {record.ThoiGianThucHienDen}
          </p>
        ),
      },

      {
        title: "Thuộc chương trình",
        dataIndex: "ThuocChuongTrinh",
        key: "ThuocChuongTrinh",
        width: 200,
      },
      {
        title: "Địa chỉ ứng dụng",
        dataIndex: "DiaChiUngDung",
        key: "DiaChiUngDung",
        width: 200,
      },
      {
        title: "File đính kèm",
        dataIndex: "FileDinhKem",
        key: "FileDinhKem",
        width: 200,
        render: (text, record, index) => {
          return record.FileDinhKem.map((item) => (
            <span className="border border-primary rounded mx-1 my-1 d-inline-block p-1">
              <a download={item.TenFileGoc} target="_blank" href={item.FileUrl} style={{ maxWidth: 100 }} className="text-truncate d-inline-block">
                {item.TenFileGoc}
              </a>
            </span>
          ));
        },
      },
      {
        title: "Trạng thái",
        dataIndex: "TrangThai",
        key: "TrangThai",
        width: 200,
        render: (text, record, index) => {
          const trangThai = TrangThai.find((d) => d.value === text);
          return <p className="mb-0">{trangThai ? (trangThai.value === 4 ? "Đã duyệt" : trangThai.label) : ""}</p>;
        },
      },
      {
        title: "Hành động",
        dataIndex: "Hanhdong",
        key: "HanhDong",
        width: 200,
        render: (text, record) => {
          return (
            <div className="text-center">
              {role.duyet && [2, 6].includes(record.TrangThai) ? (
                <div className="d-inline-block px-1">
                  <Button
                    type="primary"
                    onClick={() => {
                      this.handleOpenModal("DuyetDeXuatModal")(record);
                    }}
                  >
                    Duyệt
                  </Button>
                </div>
              ) : (
                ""
              )}

              {role.gui && [1, 3].includes(record.TrangThai) ? (
                <div className="d-inline-block px-1">
                  <Button
                    type="primary"
                    onClick={() => {
                      this.handleGuiDeXuat(record);
                    }}
                  >
                    Gửi
                  </Button>
                </div>
              ) : (
                ""
              )}
              {role.delete && [1].includes(record.TrangThai) ? (
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
        <PageHeader>Quản lý đề xuất</PageHeader>
        <PageAction>
          <div>
            {role.view ? (
              <div className="d-inline-block">
                <Button
                  className="mx-1"
                  type="primary"
                  onClick={() => {
                    this.handleExportExcel("export");
                  }}
                >
                  Xuất Excel
                </Button>
                <Button
                  className="mx-1"
                  type="primary"
                  onClick={() => {
                    this.handleExportExcel("print");
                  }}
                >
                  In danh sách
                </Button>
              </div>
            ) : (
              ""
            )}

            {role.add ? (
              <div className="d-inline-block">
                <Button type="primary" onClick={this.handleOpenModal("DeXuatModal")}>
                  Thêm mới
                </Button>
              </div>
            ) : (
              ""
            )}
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
                      Type: 1,
                      status: true,
                    },
                  }}
                  placeholder="Lĩnh vực nghiên cứu"
                  onChange={(value) => {
                    this.onFilter(value, "LinhVucNghienCuu");
                  }}
                  value={filterData.LinhVucNghienCuu}
                />
              </div>
              <div className="col-6 col-lg-2">
                <TreeSelectWithApi
                  apiConfig={{
                    api: api.danhSachCayLinhVuc,
                    valueField: "ID",
                    nameField: "Name",
                    codeField: "Code",
                    filter: {
                      Type: 2,
                      status: true,
                    },
                  }}
                  placeholder="Lĩnh vực kinh tế - xã hội"
                  onChange={(value) => {
                    this.onFilter(value, "LinhVucKinhTeXaHoi");
                  }}
                  value={filterData.LinhVucKinhTeXaHoi}
                />
              </div>
              <div className="col-6 col-lg-2">
                <TreeSelectWithApi
                  apiConfig={{
                    api: api.danhSachCayCapQuanly,
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
              <div className="col-6 col-lg-2">
                <Select
                  allowClear
                  placeholder="Trạng thái"
                  onChange={(value) => {
                    this.onFilter(value, "TrangThai");
                  }}
                  value={filterData.TrangThai ? Number(filterData.TrangThai) : undefined}
                >
                  {TrangThai.map((item) => (
                    <Select.Option value={item.value} key={item.key}>
                      {item.value === 4 ? "Đã duyệt" : item.label}
                    </Select.Option>
                  ))}
                </Select>
              </div>
              {role.qlDonVi || role.qlToanTruong ? (
                <div className="col-12 col-lg-2">
                  <SelectWithApi
                    apiConfig={{
                      api: api.DanhSachTaiKhoan,
                      valueField: "CanBoID",
                      nameField: "TenCanBo",
                      filter: {
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

              <div className="col-12 col-lg-2">
                <Input
                  value={filterData.Keyword}
                  onChange={this.handleChangeKeyword}
                  onPressEnter={(e) => {
                    this.onFilter(e.target.value, "Keyword");
                  }}
                  placeholder="Nhập mã hoặc tên đề xuất cần tìm kiếm"
                  allowClear
                  suffix={<Icon type="search" style={{ color: "rgba(0,0,0,.45)" }} />}
                ></Input>
              </div>
            </div>
          </ValidatorForm>
          <BoxTable
            update={this.state.update}
            columns={columns}
            rowKey="DeXuatID"
            dataSource={listDeXuat}
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
              this.setState({ excelData: null, command: "" });
            }}
            command={this.state.command}
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
  const role = getRoleByKey2("ql-de-xuat");
  const role2 = getRoleByKey2("de-xuat");
  const role3 = getRoleByKey2("duyet-de-xuat");
  const role4 = getRoleByKey2("ql-toan-truong");
  const role5 = getRoleByKey2("ql-don-vi");
  state.QLDeXuat.role = { ...role, ...{ duyet: role3.view, gui: role2.view, qlToanTruong: role4.view, qlDonVi: role5.view } };

  // state.QLDeXuat.role = { view: 1, edit: 1, add: 1, ...{ duyet: 1, gui: 1, qlToanTruong: 1, qlDonVi: 1 } };
  return {
    ...state.QLDeXuat,
  };
}
export default connect(mapStateToProps, actions)(QLDeXuat);
