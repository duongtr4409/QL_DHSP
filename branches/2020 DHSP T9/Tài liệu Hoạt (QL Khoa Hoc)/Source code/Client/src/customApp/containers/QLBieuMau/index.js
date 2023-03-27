import React, {Component} from "react";
import {connect} from "react-redux";
import {Icon, Input, Modal, message, Tooltip} from "antd";
import LayoutWrapper from "../../../components/utility/layoutWrapper";
import PageHeader from "../../../components/utility/pageHeader";
import Box from "../../../components/utility/box";
import BoxFilter from "../../../components/utility/boxFilter";
import BoxTable from '../../../components/utility/boxTable';
import PageAction from "../../../components/utility/pageAction";
import Button from "../../../components/uielements/button";
import actions from "../../redux/QLBieuMau/actions";
import {changeUrlFilter, getFilterData, getDefaultPageSize} from "../../../helpers/utility";
import queryString from 'query-string';
import api, {apiUrl} from "./config";
import {ModalAddEdit} from "./modalAddEditBieuMau";
import moment from "moment";
import {formDataCaller} from '../../../helpers/formDataCaller'

const {Search} = Input;

class QLBieuMau extends Component {
  constructor(props) {
    document.title = 'Quản lý biểu mẫu';
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

  deleteBieuMau = () => {
    const {selectedRowKeys} = this.state;
    this.setState({loading: false});
    Modal.confirm({
      title: 'Thông báo',
      content: 'Bạn chắc chắn muốn xóa danh mục này không ?',
      okText: 'Có',
      cancelText: 'Không',
      onOk: () => {
        api.XoaBieuMau({ListID: selectedRowKeys})
          .then(response => {
            if (response.data.Status > 0) {
              message.destroy();
              message.success('Xóa biểu mẫu thành công');
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

  editBieuMau = () => {
    const {selectedRowKeys} = this.state;
    let {modalKey} = this.state;
    modalKey++;
    const BieuMauID = selectedRowKeys[0];
    this.setState({loading: true});
    api.ChiTietBieuMau({BieuMauID: BieuMauID})
      .then(response => {
        if (response.data.Status === 1) {
          const dataEdit = response.data.Data;
          this.setState({
            dataEdit,
            loading: false,
            visibleModal: true,
            modalKey,
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

  addBieuMau = () => {
    let {modalKey} = this.state;
    modalKey++;
    this.setState({visibleModal: true, modalKey, actions: 'add', dataEdit: {}, loading: false});
  };

  closeModalAddEdit = () => {
    this.setState({visibleModal: false, selectedRowKeys: []});
  };

  submitModalAddEdit = (data, FileDinhKem) => {
    const {actions} = this.state;
    this.setState({loading: true}, () => {
      if (actions === 'add') {
        delete data.BieuMauID;
        const FormFile = new FormData();
        //Append file đính kèm
        if (FileDinhKem && FileDinhKem.FileData) {
          FormFile.append('FileDinhKem', FileDinhKem.FileData);
        }
        //Append data
        FormFile.append('DanhMucBieuMauModel', JSON.stringify(data));
        formDataCaller(apiUrl.thembieumau, FormFile).then(response => {
          if (response.data.Status === 1) {
            message.destroy();
            message.success('Thêm thông tin thành công');
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
        const FormFile = new FormData();
        //append file đính kèm
        if (FileDinhKem && FileDinhKem.FileData) {
          FormFile.append('FileDinhKem', FileDinhKem.FileData);
        }
        //append data
        FormFile.append('DanhMucBieuMauModel', JSON.stringify(data));
        formDataCaller(apiUrl.suabieumau, FormFile).then(response => {
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
    const {DanhSachBieuMau, TotalRow, TableLoading, role, FileLimit, FileAllow} = this.props;
    const PageNumber = filterData.PageNumber ? parseInt(filterData.PageNumber) : 1;
    const PageSize = filterData.PageSize ? parseInt(filterData.PageSize) : getDefaultPageSize();
    const pathFile = window.location.host;
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
        title: 'Tên biểu mẫu',
        dataIndex: 'TenBieuMau',
        width: '25%',
      },
      {
        title: 'Thời gian cập nhật',
        width: '20%',
        render: (text, record) => (
          record.NgayCapNhat ? moment(record.NgayCapNhat).format("DD/MM/YYYY") : ""
        ),
        align: 'center'
      },
      {
        title: 'Người đăng',
        dataIndex: 'NguoiCapNhatStr',
        width: '20%',
      },
      {
        title: 'Ghi chú',
        dataIndex: 'GhiChu',
        width: '20%'
      },
      {
        title: 'Tải file',
        width: '15%',
        align: 'center',
        render: (text, record) => (
          record.FileDinhKem.FileDinhKemID ?
            <Tooltip title={record.FileDinhKem.TenFileGoc}>
              <a href={record.FileDinhKem.FileUrl} download={record.FileDinhKem.TenFileGoc} target={'_blank'}>
                <Icon type="cloud-download"
                      style={{fontSize: 30}}/>
              </a>
            </Tooltip> : ""
        )
      }
    ];
    return (
      <LayoutWrapper>
        <PageHeader>Quản lý biểu mẫu</PageHeader>
        <PageAction>
          {role.delete ? <Button type={'primary'} style={{marginRight: 5}} onClick={this.deleteBieuMau}
                                 disabled={selectedRowKeys.length < 1}>Xóa</Button> : ""}
          {role.edit ? <Button type={'primary'} style={{marginRight: 5}} onClick={this.editBieuMau}
                               disabled={selectedRowKeys.length !== 1}>Sửa</Button> : ""}
          {role.add ? <Button type={'primary'} onClick={this.addBieuMau}>Thêm mới</Button> : ""}
        </PageAction>
        <Box>
          <BoxFilter>
            <Search placeholder={'Nhập tên biểu mẫu cần tìm kiếm'} style={{width: 300, marginLeft: 10}}
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
            rowKey={'BieuMauID'}
            dataSource={DanhSachBieuMau}
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
                      onCreate={this.submitModalAddEdit} loading={loading} actions={actions} FileLimit={FileLimit}
                      FileAllow={FileAllow}/>
      </LayoutWrapper>
    )
  }
}

function mapStateToProps(state) {
  return {
    ...state.QLBieuMau
  };
}

export default connect(
  mapStateToProps,
  actions
)(QLBieuMau);