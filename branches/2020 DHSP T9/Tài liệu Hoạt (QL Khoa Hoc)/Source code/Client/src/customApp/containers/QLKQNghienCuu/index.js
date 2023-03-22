import React, { Component } from "react";
import api from "./config";
import { connect } from "react-redux";
import * as actions from "../../redux/QLKQNghienCuu/actions";
import LayoutWrapper from "../../../components/utility/layoutWrapper";
import PageHeader from "../../../components/utility/pageHeader";
import PageAction from "../../../components/utility/pageAction";
import { changeUrlFilter, getDefaultPageSize, getFilterData, getOptionSidebar } from "../../../helpers/utility";
import queryString from "query-string";
import Box from "../../../components/utility/box";
import BoxFilter from "../../../components/utility/boxFilter";
import BoxTable, { EmptyTable } from "../../../components/utility/boxTable";
import { Modal } from "antd";
import KQNghienCuuModal from "./KQNghienCuuModal";
class QLKQNghienCuu extends Component {
  constructor(props) {
    super(props);
    const filterData = queryString.parse(this.props.location.search);
    this.state = {
      filterData: filterData,
      selectedRowKeys: [],
    };
  }
  modal = null;
  //Get initData---------------------------------------------
  componentDidMount = () => {
    // this.props.getInitData(this.state.filterData);
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
        this.resetConfig("close"); //reset configData
      }
    );
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
        this.resetConfig("close"); //reset configData
      }
    );
  };

  handleCloseModal = () => {
    this.modal.destroy();
  };

  handleAdd = () => {
    this.modal = Modal.confirm({
      icon: <i />,
      content: <KQNghienCuuModal data={null} onClose={this.handleCloseModal}></KQNghienCuuModal>,
    });
  };

  render() {
    const { DSKQNghienCuu, TotalRow } = this.props;
    const { filterData } = this.state;
    const PageNumber = filterData.PageNumber ? parseInt(filterData.PageNumber) : 1;
    const PageSize = filterData.PageSize ? parseInt(filterData.PageSize) : getDefaultPageSize();
    const columns = [
      {
        title: "STT",
        align: "center",
        width: 50,
        render: (text, record, index) => <span>{(PageNumber - 1) * PageSize + (index + 1)}</span>,
      },
      {
        title: "Tên đề tài khoa học",
        dataIndex: "TenNhom",
        key: "TenNhom",
        width: "32%",
      },
      {
        title: "Loại kết quả",
        dataIndex: "LoaiKetQua",
        key: "LoaiKetQua",
      },
      {
        title: "Nội dung kết quả nghiên cứu",
        dataIndex: "GhiChu",
        key: "GhiChu",
      },
      {
        title: "File đính kèm",
        dataIndex: "File",
        key: "File",
      },
    ];
    return (
      <LayoutWrapper>
        <PageHeader>Quản lý kết quả nghiên cứu</PageHeader>
        <PageAction>
          <div>
            <div className="btn btn-primary pointer btn-sm" onClick={this.handleAdd}>
              Thêm mới
            </div>
          </div>
        </PageAction>
        <Box>
          <BoxTable
            // rowSelection={{
            //   onChange: this.onSelectChange,
            //   selectedRowKeys: this.state.selectedRowKeys,
            //   columnWidth: 50,
            // }}
            columns={columns}
            rowKey="NhomNguoiDungID"
            dataSource={DSKQNghienCuu}
            // loading={this.props.TableLoading}
            // onChange={this.onTableChange}
            pagination={{
              showSizeChanger: true, //show text: PageSize/page
              showTotal: (total, range) => `Từ ${range[0]} đến ${range[1]} trên ${total} kết quả`,
              total: TotalRow,
              current: PageNumber, //current page
              pageSize: PageSize,
            }}
            scroll={{ y: "65vh" }}
          />
        </Box>
      </LayoutWrapper>
    );
  }
}
function mapStateToProps(state) {
  return {
    ...state.QLKQNghienCuu,
  };
}
export default connect(mapStateToProps, actions)(QLKQNghienCuu);
