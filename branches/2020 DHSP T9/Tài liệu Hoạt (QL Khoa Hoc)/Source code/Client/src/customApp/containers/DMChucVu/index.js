import React, {Component} from "react";
import {connect} from "react-redux";
import {Input, Modal, message} from "antd";
import LayoutWrapper from "../../../components/utility/layoutWrapper";
import PageHeader from "../../../components/utility/pageHeader";
import Box from "../../../components/utility/box";
import BoxFilter from "../../../components/utility/boxFilter";
import BoxTable from '../../../components/utility/boxTable';
import PageAction from "../../../components/utility/pageAction";
import Button from "../../../components/uielements/button";
import actions from "../../redux/DMChucVu/actions";
import {ModalAddEdit} from "./modalAddEditChucVu";
import queryString from "query-string";
import {changeUrlFilter, getDefaultPageSize, getFilterData} from "../../../helpers/utility";
import api from './config'

const {Search} = Input;

class DMChucVu extends Component {
  constructor(props) {
    document.title = 'Danh mục chức vụ';
    super(props);
    const filterData = queryString.parse(this.props.location.search);
    this.state = {
      selectedRowKeys: [],
      modalKey: 0,
      visibleModal: false,
      dataEdit: {},
      actions: "",
      filterData: filterData,
      loading: false
    }
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

  deleteChucVu = () => {
    Modal.confirm({
      title: 'Thông báo',
      content: 'Bạn chắc chắn muốn xóa chức vụ này không ?',
      okText: 'Có',
      cancelText: 'Không',
      onOk: () => {
        const {selectedRowKeys} = this.state;
        api.XoaChucVu({ListID: selectedRowKeys})
          .then(response => {
            if (response.data.Status === 1) {
              this.setState({selectedRowKeys: []});
              this.props.getList(this.state.filterData);
              message.destroy();
              message.success('Xóa chức vụ thành công');
            } else {
              this.props.getList(this.state.filterData);
              message.destroy();
              message.error(response.data.Message);
            }
          }).catch(error => {
          message.destroy();
          message.error(error.toString());
        })
      },
      onCancel: () => {
        this.setState({loading: false})
      }
    });
  };

  editChucVu = () => {
    const {selectedRowKeys} = this.state;
    const ChucVuID = selectedRowKeys[0];
    this.setState({loading: true});
    api.ChiTietChucVu({ChucVuID: ChucVuID})
      .then(response => {
        if (response.data.Status === 1) {
          const dataEdit = response.data.Data;
          this.setState({
            dataEdit,
            modalKey: ++this.state.modalKey,
            visibleModal: true,
            actions: 'edit',
            loading: false
          })
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

  addChucVu = () => {
    this.setState({visibleModal: true, modalKey: ++this.state.modalKey, actions: 'add', dataEdit: {}, loading: false});
  };

  closeModalAddEdit = () => {
    this.setState({visibleModal: false});
  };

  submitModalAddEdit = (data) => {
    const {actions} = this.state;
    this.setState({loading: true});
    if (actions === 'add') {
      delete data.ChucVuID;
      api.ThemChucVu(data)
        .then(response => {
          if (response.data.Status === 1) {
            message.destroy();
            message.success('Thêm chức vụ thành công');
            this.setState({visibleModal: false});
            this.props.getList(this.state.filterData);
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
    } else if (actions === 'edit') {
      api.SuaChucVu(data)
        .then(response => {
          if (response.data.Status === 1) {
            message.destroy();
            message.success('Sửa thông tin chức vụ thành công');
            this.setState({visibleModal: false, selectedRowKeys: []});
            this.props.getList(this.state.filterData);
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
    }
  };

  render() {
    const {modalKey, visibleModal, dataEdit, selectedRowKeys, filterData, loading} = this.state;
    const {DanhSachChucVu, TableLoading, TotalRow, role} = this.props;
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
        title: 'Chức vụ',
        dataIndex: 'TenChucVu',
        width: '70%',
        sorter: 'TenChucVu'
      },
      {
        title: 'Ghi chú',
        dataIndex: 'GhiChu',
        width: '25%'
      }
    ];
    return (
      <LayoutWrapper>
        {/*<div className={'LayoutContentScroll'} style={{width: '100%'}}>*/}
        <PageHeader>Danh mục chức vụ</PageHeader>
        <PageAction>
          {role.delete ? <Button type={'primary'} style={{marginRight: 5}} onClick={this.deleteChucVu}
                                 disabled={selectedRowKeys.length < 1}>Xóa</Button> : ""}
          {role.edit ? <Button type={'primary'} style={{marginRight: 5}} onClick={this.editChucVu}
                               disabled={selectedRowKeys.length !== 1}>Sửa</Button> : ""}
          {role.add ? <Button type={'primary'} onClick={this.addChucVu}>Thêm
            mới</Button> : ""}
        </PageAction>
        <Box>
          <BoxFilter>
            <Search placeholder={'Nhập tên chức vụ cần tìm kiếm'} style={{width: 300}} allowClear
                    onSearch={value => this.onSearch(value, 'Keyword')} defaultValue={filterData.Keyword}/>
          </BoxFilter>
          <BoxTable
            rowSelection={{
              onChange: (selectedRowKeys, selectedRows) => {
                this.setState({selectedRowKeys});
              },
              selectedRowKeys: this.state.selectedRowKeys
            }}
            columns={column}
            rowKey={'ChucVuID'}
            dataSource={DanhSachChucVu}
            pagination={{
              showSizeChanger: true, //show text: pageSize/page
              showTotal: (total, range) => `Từ ${range[0]} đến ${range[1]} trên ${total}`,
              total: TotalRow, //test 100
              current: PageNumber, //current page
              pageSize: PageSize,
            }}
            loading={TableLoading}
            onChange={this.onTableChange}
            scroll={{y: '65vh'}}
          />
        </Box>
        {/*</div>*/}
        <ModalAddEdit visible={visibleModal}
                      dataEdit={dataEdit} onCancel={this.closeModalAddEdit} key={modalKey}
                      onCreate={this.submitModalAddEdit} loading={loading}/>
      </LayoutWrapper>
    )
  }
}

function mapStateToProps(state) {
  return {
    ...state.DMChucVu
  };
}

export default connect(
  mapStateToProps,
  actions
)(DMChucVu);