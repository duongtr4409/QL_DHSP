import React, { Component } from "react";
import { connect } from "react-redux";
import { Icon, Input, Modal, message, TreeSelect, Select } from "antd";
import LayoutWrapper from "../../../components/utility/layoutWrapper";
import PageHeader from "../../../components/utility/pageHeader";
import Box from "../../../components/utility/box";
import BoxFilter from "../../../components/utility/boxFilter";
import BoxTable from "../../../components/utility/boxTable";
import PageAction from "../../../components/utility/pageAction";
import Button from "../../../components/uielements/button";
import actions from "../../redux/QLTaiKhoan/actions";
import { ModalAddEdit } from "./modalAddEditTaiKhoan";
import moment from "moment";
import defaultAvatar from "../../../image/defaultAvatar.jpeg";
import { changeUrlFilter, getFilterData, getDefaultPageSize } from "../../../helpers/utility";
import queryString from "query-string";
import api, { apiUrl } from "./config";
import { formDataCaller } from "../../../helpers/formDataCaller";
// import Select, { Option } from "../../../components/uielements/select";

const { Search } = Input;

class QLTaiKhoan extends Component {
  constructor(props) {
    document.title = "Quản lý tài khoản";
    super(props);
    const filterData = queryString.parse(this.props.location.search);
    this.state = {
      selectedRowKeys: [],
      selectedRows: null,
      modalKey: 0,
      visibleModal: false,
      dataEdit: {},
      actions: "",
      loading: false,
      success: false,
      filterData: filterData,
      NguoiDungID: undefined,
      DanhSachNhomNguoiDung: [],
    };
  }

  componentDidMount() {
    this.props.getInitData(this.state.filterData);
  }

  onSearch = (value, property) => {
    let oldFilterData = this.state.filterData;
    if(typeof value == 'string')
    {
      value = value.trim();
    }
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

  onTableChange = (pagination, filters, sorter) => {
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

  deleteTaiKhoan = () => {
    const user_id = JSON.parse(localStorage.getItem("user")).CanBoID;
    const { selectedRowKeys } = this.state;
    this.setState({ loading: false });
    if (selectedRowKeys.includes(user_id)) {
      message.destroy();
      message.warning("Không thể xóa thông tin của bản thân");
      return;
    }
    if (this.state.selectedRows.findIndex((d) => d.LaCanBoTrongTruong) !== -1) {
      message.warning("Không thể xóa cán bộ trong trường");
      return;
    }

    Modal.confirm({
      title: "Thông báo",
      content: "Bạn chắc chắn muốn xóa tài khoản này không ?",
      okText: "Có",
      cancelText: "Không",
      onOk: () => {
        api
          .XoaTaiKhoan({ ListID: selectedRowKeys.map((item) => JSON.parse(item).CanBoID) })
          .then((response) => {
            if (response.data.Status > 0) {
              message.destroy();
              message.success("Xóa tài khoản thành công");
              this.setState({ loading: false, selectedRowKeys: [] });
              this.props.getList(this.state.filterData);
            } else {
              const errors = response.data.Data;

              message.destroy();
              message.error(
                errors.map((item) => {
                  return <div>{item}</div>;
                })
              );
              this.setState({ loading: false, selectedRowKeys: [] });
              this.props.getList(this.state.filterData);
            }
          })
          .catch((error) => {
            this.setState({ loading: false, selectedRowKeys: [] });
            message.error(error.toString());
            message.destroy();
          });
      },
    });
  };

  editTaiKhoan = () => {
    const { selectedRowKeys } = this.state;
    const params = JSON.parse(selectedRowKeys[0]);

    this.setState({ loading: true });
    api
      .ChiTietTaiKhoan({ CanBoID: params.CanBoID })
      .then((response) => {
        if (response.data.Status === 1) {
          const dataEdit = response.data.Data;
          dataEdit.NgaySinh = dataEdit.NgaySinh ? moment(dataEdit.NgaySinh) : null;
          this.setState({
            dataEdit,
            loading: false,
            visibleModal: true,
            modalKey: ++this.state.modalKey,
            actions: "edit",
          });
        } else {
          this.setState({ loading: false });
          message.destroy();
          message.error(response.data.Message);
        }
      })
      .catch((error) => {
        this.setState({ loading: false });
        message.destroy();
        message.error(error.toString());
      });
  };

  addTaiKhoan = () => {
    let { modalKey } = this.state;
    modalKey++;
    this.setState({
      visibleModal: true,
      modalKey,
      actions: "add",
      dataEdit: {},
      loading: false,
    });
  };

  closeModalAddEdit = () => {
    this.setState({ visibleModal: false, selectedRowKeys: [] });
  };

  submitModalAddEdit = (data, AnhDaiDien) => {
    data.NgaySinh = data.NgaySinh !== "" ? moment(data.NgaySinh).format("YYYY-MM-DD") : "";
    const { actions } = this.state;
    const form = new FormData();
    form.append("files", AnhDaiDien.FileData);
    form.append("HeThongCanBo", JSON.stringify(data));
    this.setState({ loading: true }, () => {
      if (actions === "add") {
        delete data.CanBoID;
        formDataCaller(apiUrl.themtaikhoan, form)
          .then((response) => {
            if (response.data.Status === 1) {
              message.destroy();
              message.success("Thêm thông tin thành công");
              this.props.getList(this.state.filterData);
              this.setState({ visibleModal: false, selectedRowKeys: [], success: true });
            } else {
              message.destroy();
              message.error(response.data.Message);
              this.setState({ loading: false });
            }
          })
          .catch((error) => {
            this.setState({ loading: false });
            message.destroy();
            message.error(error.toString());
          });
      } else if (actions === "edit") {
        formDataCaller(apiUrl.suataikhoan, form)
          .then((response) => {
            if (response.data.Status === 1) {
              message.destroy();
              message.success("Sửa thông tin thành công");
              this.props.getList(this.state.filterData);
              this.setState({ visibleModal: false, selectedRowKeys: [], success: true });
            } else {
              message.destroy();
              message.error(response.data.Message);
              this.setState({ loading: false });
            }
          })
          .catch((error) => {
            this.setState({ loading: false });
            message.destroy();
            message.error(error.toString());
          });
      }
    });
  };

  renderCoQuan = (coquanid) => {
    const { DanhSachCoQuan } = this.props;
    let TenCoQuan = "";
    if (coquanid) {
      const CoQuan = DanhSachCoQuan.filter((item) => item.CoQuanID === coquanid);
      if (CoQuan.length > 0) {
        TenCoQuan = CoQuan[0].TenCoQuan;
      }
    }
    return TenCoQuan;
  };

  resetPassword = () => {
    const { selectedRowKeys } = this.state;
    const { DanhSachTaiKhoan } = this.props;
    let NguoiDungID;
    if (selectedRowKeys.length !== 1) {
      NguoiDungID = undefined;
    } else {
      const dataFilter = DanhSachTaiKhoan.filter((item) => item.CanBoID === JSON.parse(selectedRowKeys[0]).CanBoID);
      if (dataFilter.length < 1) {
        NguoiDungID = undefined;
      } else {
        NguoiDungID = dataFilter[0].NguoiDungID;
      }
    }
    if (!NguoiDungID) {
      return;
    }
    const { MatKhauMacDinh } = this.props;
    Modal.confirm({
      title: "Thông báo",
      content: "Bạn có muốn đặt lại mật khẩu không ?",
      okText: "Có",
      cancelText: "Không",
      onOk: () => {
        api
          .ResetMatKhau({ NguoiDungID: NguoiDungID })
          .then((response) => {
            if (response.data.Status > 0) {
              Modal.success({
                title: "Thông báo",
                content: `Mật khẩu đã được chuyển về mặc định là: ${MatKhauMacDinh}`,
                okText: "Đóng",
                onOk: () => {
                  this.setState({ selectedRowKeys: [] });
                },
              });
            } else {
              message.destroy();
              message.error(response.data.Message);
            }
          })
          .catch((error) => {
            message.destroy();
            message.error(error.toString());
          });
      },
    });
  };

  render() {
    const { modalKey, visibleModal, dataEdit, selectedRowKeys, loading, filterData, actions, selectedRows } = this.state;
    const { DanhSachTaiKhoan, DanhSachCoQuan, TotalRow, TableLoading, DanhSachTaiKhoanAll, role, DanhSachNhomNguoiDung, FileLimit } = this.props;
    const PageNumber = filterData.PageNumber ? parseInt(filterData.PageNumber) : 1;
    const PageSize = filterData.PageSize ? parseInt(filterData.PageSize) : getDefaultPageSize();
    const column = [
      {
        title: "STT",
        key: "stt",
        dataIndex: "stt",
        align: "center",
        width: "5%",
        render: (text, record, index) => <span>{(PageNumber - 1) * PageSize + (index + 1)}</span>,
      },
      {
        title: "Ảnh đại diện",
        align: "center",
        width: "10%",
        render: (text, record, index) => <img alt="ava" src={record.AnhHoSo && record.AnhHoSo !== "" ? record.AnhHoSo : defaultAvatar} style={{ width: 50, height: 50, borderRadius: "50%" }} />,
      },
      {
        title: "Tên đăng nhập",
        key: "dangNhap",
        dataIndex: "TenNguoiDung",
        width: "20%",
      },
      {
        title: "Email",
        key: "Email",
        dataIndex: "Email",
        width: "20%",
      },
      {
        title: "Tên cán bộ",
        key: "tenCanBo",
        dataIndex: "TenCanBo",
        width: "20%",
      },
      {
        title: "Cơ quan",
        key: "coQuan",
        dataIndex: "TenCoQuan",
        width: "25%",
        render: (text, record) => <span>{this.renderCoQuan(record.CoQuanID)}</span>,
      },
    ];
    return (
      <LayoutWrapper>
        <PageHeader>Quản lý tài khoản</PageHeader>
        <PageAction>
          {role.edit ? (
            <Button
              style={{
                marginRight: 5,
                background: selectedRowKeys.length !== 1 ? "" : "#00a65a",
                color: selectedRowKeys.length !== 1 ? "" : "white",
              }}
              onClick={this.resetPassword}
              disabled={selectedRowKeys.length !== 1}
            >
              Reset mật khẩu
            </Button>
          ) : (
            ""
          )}
          {role.delete ? (
            <Button type={"primary"} style={{ marginRight: 5 }} onClick={this.deleteTaiKhoan} disabled={selectedRowKeys.length < 1 || (selectedRows[0] && selectedRows[0].LaCanBoTrongTruong)}>
              Xóa
            </Button>
          ) : (
            ""
          )}
          {role.edit ? (
            <Button type={"primary"} style={{ marginRight: 5 }} onClick={this.editTaiKhoan} disabled={selectedRowKeys.length !== 1 || (selectedRows[0] && selectedRows[0].LaCanBoTrongTruong)}>
              Sửa
            </Button>
          ) : (
            ""
          )}
          {role.add ? (
            <Button type={"primary"} onClick={this.addTaiKhoan}>
              Thêm mới
            </Button>
          ) : (
            ""
          )}
        </PageAction>
        <Box>
          <BoxFilter>
            <div className="row align-items-center">
              <div className="col-12 col-lg-3 my-1">
                <Select notFoundContent="Không có dữ liệu" optionFilterProp={"children"} onChange={(value) => this.onSearch(value, "CoQuanID")} showSearch placeholder="Chọn cơ quan" allowClear className="w-100">
                  {DanhSachCoQuan.map((value) => (
                    <Select.Option key={value.CoQuanID} value={value.CoQuanID}>
                      {`${value.TenCoQuan}`}
                    </Select.Option>
                  ))}
                </Select>
              </div>
              <div className="col-12 col-lg-4">
                <Search placeholder={"Nhập tên cán bộ hoặc tài khoản cần tìm kiếm"} allowClear onSearch={(value) => this.onSearch(value, "Keyword")} defaultValue={filterData.Keyword} />
              </div>
            </div>
          </BoxFilter>
          <BoxTable
            rowSelection={{
              onChange: (selectedRowKeys, selectedRows) => {
                this.setState({ selectedRowKeys, selectedRows });
              },
              selectedRowKeys: this.state.selectedRowKeys,
            }}
            columns={column}
            rowKey={(record) => JSON.stringify({ CanBoID: record.CanBoID, CoQuanID: record.CoQuanID })}
            dataSource={DanhSachTaiKhoan}
            pagination={{
              showSizeChanger: true, //show text: pageSize/page
              showTotal: (total, range) => `Từ ${range[0]} đến ${range[1]} trên ${total}`,
              total: TotalRow, //test 100
              current: PageNumber, //current page
              pageSize: PageSize,
            }}
            loading={TableLoading}
            onChange={this.onTableChange}
          />
        </Box>
        <ModalAddEdit visible={visibleModal} DanhSachNhomNguoiDung={DanhSachNhomNguoiDung} dataEdit={dataEdit} onCancel={this.closeModalAddEdit} key={modalKey} onCreate={this.submitModalAddEdit} loading={loading} DanhSachTaiKhoanAll={DanhSachTaiKhoanAll} actions={actions} FileLimit={FileLimit} />
      </LayoutWrapper>
    );
  }
}

function mapStateToProps(state) {
  return {
    ...state.QLTaiKhoan,
  };
}

export default connect(mapStateToProps, actions)(QLTaiKhoan);
