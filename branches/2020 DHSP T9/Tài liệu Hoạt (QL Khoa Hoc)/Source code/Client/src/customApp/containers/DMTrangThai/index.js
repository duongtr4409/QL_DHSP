import React, {Component} from "react";
import {connect} from "react-redux";
import {Icon, Input, Modal, message} from "antd";
import LayoutWrapper from "../../../components/utility/layoutWrapper";
import PageHeader from "../../../components/utility/pageHeader";
import Box from "../../../components/utility/box";
import BoxFilter from "../../../components/utility/boxFilter";
import BoxTable from '../../../components/utility/boxTable';
import PageAction from "../../../components/utility/pageAction";
import Button from "../../../components/uielements/button";
import actions from "../../redux/DMTrangThai/actions";
import {changeUrlFilter, getFilterData, getDefaultPageSize} from "../../../helpers/utility";
import queryString from 'query-string';
import api from "./config";
import {ModalAddEdit} from "./modalAddEditTrangThai";

const {Search} = Input;

class DMTrangThai extends Component {
  constructor(props) {
    document.title = 'Danh mục trạng thái';
    super(props);
    const filterData = queryString.parse(this.props.location.search);
    this.state = {
      selectedRowKeys: [],
      modalKey: 0,
      visibleModal: false,
      dataEdit: {},
      actions: "",
      loading: false,
      filterData: filterData
    }
  }

  componentDidMount() {
    this.props.getInitData(this.state.filterData);
  }

  onSearch = (value, property) => {
    let oldFilterData = this.state.filterData;
    let onFilter = {value, property};
    let filterData = getFilterData(oldFilterData, onFilter, null);
    //get filter data
    this.setState(
      {
        filterData,
        selectedRowKeys: []
      },
      () => {
        changeUrlFilter(this.state.filterData); //change url
        this.props.getList(this.state.filterData); //get list
      }
    );
  };

  onTableChange = (pagination, filters, sorter) => {
    let oldFilterData = this.state.filterData;
    let onOrder = {pagination, filters, sorter};
    let filterData = getFilterData(oldFilterData, null, onOrder);
    //get filter data
    this.setState(
      {
        filterData,
        selectedRowKeys: []
      },
      () => {
        changeUrlFilter(this.state.filterData); //change url
        this.props.getList(this.state.filterData); //get list
      }
    );
  };

  deleteTrangThai = () => {
    const {selectedRowKeys} = this.state;
    this.setState({loading: false});
    Modal.confirm({
      title: 'Thông báo',
      content: 'Bạn chắc chắn muốn xóa trạng thái này không ?',
      okText: 'Có',
      cancelText: 'Không',
      onOk: () => {
        api.XoaTrangThai({ListID: selectedRowKeys})
          .then(response => {
            if (response.data.Status > 0) {
              message.destroy();
              message.success('Xóa thành công');
              this.setState({loading: false, selectedRowKeys: []});
              this.props.getList(this.state.filterData);
            } else {
              message.destroy();
              message.error(response.data.Message);
              this.setState({loading: false, selectedRowKeys: []});
              this.props.getList(this.state.filterData);
            }
          }).catch(error => {
          this.setState({loading: false, selectedRowKeys: []});
          message.destroy();
          message.error(error.toString());
        })
      }
    });
  };

  editTrangThai = () => {
    const {selectedRowKeys} = this.state;
    let {modalKey} = this.state;
    modalKey++;
    const TrangThaiID = selectedRowKeys[0];
    this.setState({loading: true});
    api.ChiTietTrangThai({TrangThaiID: TrangThaiID})
      .then(response => {
        if (response.data.Status === 1) {
          const dataEdit = response.data.Data;
          this.setState({
            dataEdit,
            loading: false,
            visibleModal: true,
            modalKey: modalKey,
            actions: 'edit',
          });
        } else {
          this.setState({loading: false});
          message.destroy();
          message.error(response.data.Message);
        }
      }).catch(error => {
      this.setState({loading: false});
      message.destroy();
      message.error(error.toString());
    })
  };

  addTrangThai = () => {
    let {modalKey} = this.state;
    modalKey++;
    this.setState({visibleModal: true, modalKey, actions: 'add', dataEdit: {}, loading: false});
  };

  closeModalAddEdit = () => {
    this.setState({visibleModal: false, selectedRowKeys: []});
  };

  submitModalAddEdit = (data) => {
    const {actions} = this.state;
    this.setState({loading: true}, () => {
      if (actions === 'add') {
        delete data.TrangThaiID;
        api.ThemTrangThai(data)
          .then(response => {
            if (response.data.Status === 1) {
              message.destroy();
              message.success('Thêm danh mục thành công');
              this.props.getList(this.state.filterData);
              this.setState({visibleModal: false, selectedRowKeys: [], success: true})
            } else {
              message.destroy();
              message.error(response.data.Message);
              this.setState({loading: false});
            }
          }).catch(error => {
          this.setState({loading: false});
          message.destroy();
          message.error(error.toString())
        })
      } else if (actions === 'edit') {
        api.SuaTrangThai(data)
          .then(response => {
            if (response.data.Status === 1) {
              message.destroy();
              message.success('Sửa thông tin thành công');
              this.props.getList(this.state.filterData);
              this.setState({visibleModal: false, selectedRowKeys: [], success: true})
            } else {
              message.destroy();
              message.error(response.data.Message);
              this.setState({loading: false});
            }
          }).catch(error => {
          this.setState({loading: false});
          message.destroy();
          message.error(error.toString())
        })
      }
    });
  };

  render() {
    const {modalKey, visibleModal, dataEdit, selectedRowKeys, loading, filterData, actions} = this.state;
    const {DanhSachTrangThai, TotalRow, TableLoading, role} = this.props;
    const PageNumber = filterData.PageNumber ? parseInt(filterData.PageNumber) : 1;
    const PageSize = filterData.PageSize ? parseInt(filterData.PageSize) : getDefaultPageSize();
    const column = [
      {
        title: 'STT',
        key: 'stt',
        dataIndex: 'stt',
        align: 'center',
        width: '5%',
        render: (text, record, index) => (
          <span>{(PageNumber - 1) * PageSize + (index + 1)}</span>
        )
      },
      {
        title: 'Tên trạng thái',
        dataIndex: 'TenTrangThai',
        width: '60%',
      },
      {
        title: 'Ghi chú',
        dataIndex: 'GhiChu',
        width: '25%'
      },
      {
        title: 'Trạng thái sử dụng',
        width: '10%',
        align: 'center',
        render: (text, record, index) => {
          return <span>{record.TrangThaiSuDung ? <Icon type="check"/> : ""}</span>
        }
      }
    ];
    return (
      <LayoutWrapper>
        <PageHeader>Danh mục trạng thái</PageHeader>
        <PageAction>
          {role.delete ? <Button type={'primary'} style={{marginRight: 5}} onClick={this.deleteTrangThai}
                                 disabled={selectedRowKeys.length < 1}>Xóa</Button> : ""}
          {role.edit ? <Button type={'primary'} style={{marginRight: 5}} onClick={this.editTrangThai}
                               disabled={selectedRowKeys.length !== 1}>Sửa</Button> : ""}
          {role.add ? <Button type={'primary'} onClick={this.addTrangThai}>Thêm mới</Button> : ""}
        </PageAction>
        <Box>
          <BoxFilter>
            <Search placeholder={'Nhập tên trạng thái cần tìm kiếm'} style={{width: 300, marginLeft: 10}}
                    allowClear
                    onSearch={value => this.onSearch(value, 'Keyword')} defaultValue={filterData.Keyword}/>
          </BoxFilter>
          <BoxTable
            rowSelection={{
              onChange: (selectedRowKeys) => {
                this.setState({selectedRowKeys});
              },
              selectedRowKeys: this.state.selectedRowKeys
            }}
            columns={column}
            rowKey={'TrangThaiID'}
            dataSource={DanhSachTrangThai}
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
        <ModalAddEdit visible={visibleModal} dataEdit={dataEdit} onCancel={this.closeModalAddEdit} key={modalKey}
                      onCreate={this.submitModalAddEdit} loading={loading} actions={actions}/>
      </LayoutWrapper>
    )
  }
}

function mapStateToProps(state) {
  return {
    ...state.DMTrangThai
  };
}

export default connect(
  mapStateToProps,
  actions
)(DMTrangThai);