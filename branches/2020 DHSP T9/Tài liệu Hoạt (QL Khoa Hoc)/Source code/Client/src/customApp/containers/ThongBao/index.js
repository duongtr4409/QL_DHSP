import React, { Component } from "react";
import api from "./config";
import { connect } from "react-redux";
import LayoutWrapper from "../../../components/utility/layoutWrapper";
import PageHeader from "../../../components/utility/pageHeader";
import PageAction from "../../../components/utility/pageAction";
import { changeUrlFilter, getDefaultPageSize, getFilterData } from "../../../helpers/utility";
import queryString from "query-string";
import Box from "../../../components/utility/box";
import BoxFilter from "../../../components/utility/boxFilter";
import BoxTable from "../../../components/utility/boxTable";
import { message, Input, Button, Modal, TreeSelect } from "antd";
import actions from "../../redux/QLThongBao/actions";
import moment from "moment";
import ModalComponent from "./modal";

const { Search } = Input;

class QLThongBao extends Component {
  modal = null;
  constructor(props) {
    super(props);
    const filterData = queryString.parse(this.props.location.search);
    this.state = {
      filterData: filterData,
      selectedRowKeys: [],
      visibleModal: false,
      dataEdit: {},
      modalKey: 0,
      actions: "",
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
        this.state.filterData.Keyword  = value;
        changeUrlFilter(this.state.filterData); //change url
        this.props.getList(this.state.filterData); //get list
      }
    );
  };
  handleCloseModal = () => {
    this.state.selectedRowKeys = [];
    this.props.getList(this.state.filterData);
    this.modal.destroy();
  };
  addThongBao = () => {
    this.modal = Modal.confirm({
      icon: <i />,
      content: <ModalComponent data={null} onClose={this.handleCloseModal}></ModalComponent>,
    });
  };

  editThongBao = () => {
    const { selectedRowKeys } = this.state;
    // let { modalKey } = this.state;
    // modalKey++;
    const data = this.selectedRows[0];
    this.modal = Modal.confirm({
      icon: <i />,
      content: <ModalComponent data={data} onClose={this.handleCloseModal}></ModalComponent>,
    });
  };

  XoaThongBao = () => {
    const { selectedRowKeys } = this.state;

    Modal.confirm({
      title: "Thông báo",
      content: "Bạn chắc chắn muốn xóa thông báo này không ?",
      okText: "Có",
      cancelText: "Không",
      onOk: () => {
        api
          .XoaThongBao({ ThongBaoID: this.selectedRows[0].ThongBaoID })
          .then((response) => {
            if (response.data.Status > 0) {
              message.destroy();
              message.success("Xóa thành công");
              this.setState({ loading: false, selectedRowKeys: [] });
              this.selectedRows = [];
              this.props.getList(this.state.filterData);
            } else {
              message.destroy();
              message.error(response.data.Message);
              this.setState({ loading: false, selectedRowKeys: [] });
              this.selectedRows = [];
              this.props.getList(this.state.filterData);
            }
          })
          .catch((error) => {
            this.setState({ loading: false, selectedRowKeys: [] });
            message.destroy();
            message.error(error.toString());
          });
      },
    });
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

  renderDoiTuong = (dsdoituong) => {
    const { DanhSachCanBo } = this.props;
    const DanhSachDoiTuong = [];
    // dsdoituong.forEach((item) => {
    //   const doituong = DanhSachCanBo.filter((cb) => cb.CanBoID === item.CanBoID && cb.CoQuanID === item.CoQuanID);
    //   if (doituong && doituong.length > 0) {
    //     DanhSachDoiTuong.push(doituong[0].TenCanBo);
    //   }
    // });
    return DanhSachDoiTuong.join(", ");
  };

  render() {
    const { modalKey, visibleModal, dataEdit, selectedRowKeys, loading, filterData, actions, selectedRows } = this.state;
    const { DanhSachThongBao, TotalRow, TableLoading, role } = this.props;
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
        title: "Tên thông báo",
        dataIndex: "TenThongBao",
        width: "25%",
      },
      {
        title: "Nội dung",
        dataIndex: "NoiDung",
        width: "25%",
      },
      {
        title: "Đối tượng thông báo",
        dataIndex: "TenDoiTuongThongBao",
        width: "25%",
        // render: (record, index, text) => this.renderDoiTuong(record.DoiTuongThongBao),
      },
      {
        title: "Thời gian thông báo",
        width: "20%",
        align: "center",
        render: (record, index, text) => (
          <span>
            {moment(record.ThoiGianBatDau).format("DD/MM/YYYY ")} - {moment(record.ThoiGianKetThuc).format("DD/MM/YYYY ")}
          </span>
        ),
      },
    ];
    return (
      <LayoutWrapper>
        <PageHeader>Quản lý thông báo</PageHeader>
        <PageAction>
          {role.delete ? (
            <Button type={"primary"} style={{ marginRight: 5 }} onClick={this.XoaThongBao} disabled={selectedRowKeys.length !== 1}>
              Xóa
            </Button>
          ) : (
            ""
          )}
          {role.edit ? (
            <Button type={"primary"} style={{ marginRight: 5 }} onClick={this.editThongBao} disabled={selectedRowKeys.length !== 1}>
              Sửa
            </Button>
          ) : (
            ""
          )}
          {role.add ? (
            <Button type={"primary"} onClick={this.addThongBao}>
              Thêm mới
            </Button>
          ) : (
            ""
          )}
        </PageAction>
        <Box>
          <BoxFilter>
            <Search placeholder={"Nhập thông báo cần tìm kiếm"} allowClear style={{ width: 300 }} onSearch={(value) => this.onSearch(value, "Keyword")} defaultValue={filterData.Keyword} />
          </BoxFilter>
          <BoxTable
            rowSelection={{
              onChange: (selectedRowKeys, selectedRows) => {
                this.selectedRows = selectedRows;
                this.setState({ selectedRowKeys });
              },
              selectedRowKeys: this.state.selectedRowKeys,
            }}
            columns={column}
            rowKey={"ThongBaoID"}
            dataSource={DanhSachThongBao}
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
      </LayoutWrapper>
    );
  }
}

function mapStateToProps(state) {
  return {
    ...state.QLThongBao,
  };
}

export default connect(mapStateToProps, actions)(QLThongBao);
