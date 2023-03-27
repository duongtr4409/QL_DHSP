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
import actions from "../../redux/ThamSoHeThong/actions";
import {ModalAddEdit} from "./modalAddEditThamSo";
import queryString from "query-string";
import {changeUrlFilter, getFilterData, getDefaultPageSize} from "../../../helpers/utility";
import api from "./config";

const {Search} = Input;

class ThamSoHeThong extends Component {
  constructor(props) {
    document.title = 'Tham số hệ thống';
    super(props);
    const filterData = queryString.parse(this.props.location.search);
    this.state = {
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

  deleteThamSo = (ThamSoID) => {
    Modal.confirm({
      title: 'Thông báo',
      content: 'Bạn chắc chắn muốn xóa tham này không ?',
      okText: 'Có',
      cancelText: 'Không',
      onOk: () => {

      }
    });
  };

  editThamSo = (SystemConfigID) => {
    this.setState({loading: true});
    let {modalKey} = this.state;
    modalKey++;
    api.ChiTietThamSo({SystemConfigID: SystemConfigID})
      .then(response => {
        if (response.data.Status === 1) {
          const dataEdit = response.data.Data;
          this.setState({
            dataEdit,
            loading: false,
            visibleModal: true,
            modalKey,
            actions: 'edit'
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

  addThamSo = () => {
    let {modalKey} = this.state;
    modalKey++;
    this.setState({visibleModal: true, modalKey, actions: 'add', dataEdit: {}, loading: false});
  };

  closeModalAddEdit = () => {
    this.setState({visibleModal: false});
  };

  submitModalAddEdit = (data) => {
    const {actions} = this.state;
    this.setState({loading: true});
    if (actions === 'add') {
      delete data.SystemConfigID;
      api.ThemThamSo(data)
        .then(response => {
          if (response.data.Status === 1) {
            message.destroy();
            message.success('Thêm tham số thành công');
            this.props.getList(this.state.filterData);
            this.setState({visibleModal: false, selectedRowKeys: []})
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
      api.SuaThamSo(data)
        .then(response => {
          if (response.data.Status === 1) {
            message.destroy();
            message.success('Sửa tham số thành công');
            let {filterData} = this.state;
            if (data.ConfigKey === 'Page_Size') {
              localStorage.setItem('data_config', JSON.stringify({pageSize: data.ConfigValue}));
              filterData = {...this.state.filterData, PageSize: data.ConfigValue};
              this.setState({filterData});
            }
            this.props.getList(filterData);
            this.setState({visibleModal: false, selectedRowKeys: []})
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
  };

  render() {
    const {modalKey, visibleModal, dataEdit, actions, filterData} = this.state;
    const {DanhSachThamSo, TotalRow, TableLoading, role, defaultPageSize} = this.props;
    const PageNumber = filterData.PageNumber ? parseInt(filterData.PageNumber) : 1;
    const PageSize = filterData.PageSize ? parseInt(filterData.PageSize) : getDefaultPageSize();
    const column = [
      {
        title: 'STT',
        align: 'center',
        width: '5%',
        render: (text, record, index) => (
          <span>{(PageNumber - 1) * PageSize + (index + 1)}</span>
        )
      },
      {
        title: 'Tham số',
        dataIndex: 'ConfigKey',
        width: '25%',
        sorter: 'ConfigKey'
      },
      {
        title: 'Giá trị',
        dataIndex: 'ConfigValue',
        width: '25%',
        align: 'center',
        sorter: 'ConfigValue'
      },
      {
        title: 'Ghi chú',
        dataIndex: 'Description',
        width: '25%',
        sorter: 'Description'
      },
      {
        title: 'Thao tác',
        align: "center",
        width: '20%',
        render: (text, record) => (
          <Button type={'primary'} icon={'edit'} onClick={() => this.editThamSo(record.SystemConfigID)}/>)
      },
    ];
    return (
      <LayoutWrapper>
        <PageHeader>Tham số hệ thống</PageHeader>
        <PageAction>
          {/*<Button type={'primary'} onClick={this.deleteThamSo}>Xóa</Button>*/}
          {/*<Button type={'primary'} onClick={this.editThamSo}>Sửa</Button>*/}
          {role.add ? <Button type={'primary'} onClick={this.addThamSo}>Thêm mới</Button> : ""}
        </PageAction>
        <Box>
          <BoxFilter>
            <Search placeholder={'Nhập tham số cần tìm kiếm'} style={{width: 300}} allowClear
                    onSearch={value => this.onSearch(value, 'Keyword')} defaultValue={filterData.Keyword}/>
          </BoxFilter>
          <BoxTable
            columns={column}
            dataSource={DanhSachThamSo}
            rowKey={'SystemConfigID'}
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
                      onCreate={this.submitModalAddEdit} actions={actions}/>
      </LayoutWrapper>
    )
  }
}

function mapStateToProps(state) {
  return {
    ...state.ThamSoHeThong
  };
}

export default connect(
  mapStateToProps,
  actions
)(ThamSoHeThong);