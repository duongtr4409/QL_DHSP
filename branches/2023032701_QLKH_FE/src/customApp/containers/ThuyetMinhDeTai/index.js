import React, {Component} from "react";
import {connect} from "react-redux";
import {Icon, Input, Modal, message, Tooltip, Empty} from "antd";
import LayoutWrapper from "../../../components/utility/layoutWrapper";
import PageHeader from "../../../components/utility/pageHeader";
import Box from "../../../components/utility/box";
import BoxFilter from "../../../components/utility/boxFilter";
import PageAction from "../../../components/utility/pageAction";
import Button from "../../../components/uielements/button";
import actions from "../../redux/ThuyetMinhDeTai/actions";
import {changeUrlFilter, getFilterData, getDefaultPageSize, romanize} from "../../../helpers/utility";
import queryString from 'query-string';
import api, {apiUrl} from "./config";
import {formDataCaller} from '../../../helpers/formDataCaller'
import TreeSelectEllipsis from "../../components/TreeSelectEllipsis";
import BoxTable from './BoxTable.style';
import {ModalAddEdit} from './ModalAddEditThuyetMinh';
import {ModalDuyetThuyetMinh} from './ModalDuyetThuyetMinh';

const {Search} = Input;

class ThuyetMinhDeTai extends Component {
  constructor(props) {
    document.title = 'Thuyết minh đề tài';
    super(props);
    const filterData = queryString.parse(this.props.location.search);
    this.state = {
      selectedRowKeys: [],
      modalKey: 0,
      visibleModalThuyetMinh: false,
      visibleModalDuyet: false,
      dataEdit: {},
      actions: "",
      loading: false,
      filterData: filterData,
      DanhSachDuyetThuyetMinh: [],
      listExpandRow: [],
      isBegin: true
    }
  }

  componentDidMount() {
    this.props.getInitData(this.state.filterData);
  }

  componentWillReceiveProps(nextProps, nextContext) {
    const {DanhSachThuyetMinh} = nextProps;
    const DanhSachThuyetMinhCurr = this.props.DanhSachThuyetMinh;
    if (DanhSachThuyetMinhCurr.length === 0) {
      if (DanhSachThuyetMinh.length > 0) {
        const {listExpandRow} = this.state;
        if (listExpandRow.length === 0) {
          DanhSachThuyetMinh.forEach(item => listExpandRow.push(item.DeXuatID));
          this.setState({listExpandRow});
        }
      }
    }
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

  closeModalAddEdit = () => {
    this.setState({visibleModal: false, selectedRowKeys: []});
  };

  submitModalThuyetMinh = (data, FileDinhKem) => {
    const {actions, DeXuatID} = this.state;
    data.DeXuatID = DeXuatID;
    this.setState({loading: true});
    if (actions === 'add') {
      delete data.ThuyetMinhID;
      const FormFile = new FormData();
      //Append file đính kèm
      if (FileDinhKem && FileDinhKem.FileData) {
        FormFile.append('file', FileDinhKem.FileData);
      }
      //Append data
      FormFile.append('ThuyetMinhDeTai', JSON.stringify(data));
      formDataCaller(apiUrl.themthuyetminh, FormFile).then(response => {
        if (response.data.Status === 1) {
          message.destroy();
          message.success('Thêm thông tin thành công');
          this.props.getList(this.state.filterData);
          this.setState({visibleModalThuyetMinh: false});
          this.addExpandRow(DeXuatID);
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
      if (!FileDinhKem.FileData.FileDinhKemID) {
        FormFile.append('file', FileDinhKem.FileData);
      }
      //append data
      FormFile.append('ThuyetMinhDeTai', JSON.stringify(data));
      formDataCaller(apiUrl.editthuyetminh, FormFile).then(response => {
        if (response.data.Status === 1) {
          message.destroy();
          message.success('Sửa thông tin thành công');
          this.props.getList(this.state.filterData);
          this.setState({visibleModalThuyetMinh: false});
          this.addExpandRow(DeXuatID);
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

  closeModalThuyetMinh = () => {
    this.setState({visibleModalThuyetMinh: false});
  };

  editThuyetMinh = (id) => {
    this.setState({loading: true});
    api.GetByID({ThuyetMinhID: id}).then(response => {
      if (response.data.Status > 0) {
        const dataEdit = response.data.Data;
        let {modalKey} = this.state;
        modalKey++;
        this.setState({
          loading: false,
          dataEdit,
          visibleModalThuyetMinh: true,
          modalKey,
          actions: 'edit',
          DeXuatID: dataEdit.DeXuatID
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

  addThuyetMinh = (DeXuatID) => {
    let {modalKey} = this.state;
    modalKey++;
    this.setState({visibleModalThuyetMinh: true, actions: 'add', loading: false, DeXuatID, modalKey});
  };

  deleteThuyetMinh = (id) => {
    Modal.confirm({
      title: "Thông báo",
      content: "Bạn có muốn xóa thuyết minh đề tài này không",
      okText: 'Có',
      cancelText: 'Không',
      onOk: () => {
        api.DeleteThuyetMinh(id).then(response => {
          if (response.data.Status > 0) {
            message.success('Xóa thuyết minh đề tài thành công');
            this.props.getList(this.state.filterData);
          } else {
            message.destroy();
            message.error(response.data.Message);
          }
        }).catch(error => {
          message.destroy();
          message.error(error.toString());
        })
      }
    });
  };

  duyetThuyetMinh = (DeXuatID) => {
    this.setState({loading: true});
    api.GetAllDanhSachThuyetMinh({DeXuatID: DeXuatID}).then(response => {
      if (response.data.Status > 0) {
        let {modalKey} = this.state;
        modalKey++;
        const DanhSachDuyetThuyetMinh = response.data.Data;
        this.setState({modalKey, DanhSachDuyetThuyetMinh, visibleModalDuyet: true, loading: false, DeXuatID: DeXuatID})
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

  renderTrangThai = (status) => {
    return status < 2 ? status === 0 ? 'Chờ duyệt' : 'Đã duyệt' : 'Không duyệt';
  };

  renderTenCanBo = (CanBoID, CoQuanID) => {
    const {DanhSachCanBo} = this.props;
    const canbo = DanhSachCanBo.filter(item => item.CanBoID === CanBoID && item.CoQuanID === CoQuanID)[0];
    if (canbo) {
      return canbo.TenCanBo;
    }
  };

  renderData = (data) => {
    const {loading, listExpandRow} = this.state;
    const {role, roleDuyet} = this.props;
    return data.map((item, index) =>
      <tbody>
      <tr>
        <td style={{width: '5%', textAlign: "center"}} className={'clickable'}
            onClick={() => this.onExpandRow(item.DeXuatID)}>
          {romanize(index + 1)}
        </td>
        <td colSpan={3}>
          <a onClick={() => this.viewChiTietDeXuat(item.DeXuatID)}>
            {[item.MaDeXuat, item.TenDeXuat].filter(item => item != "").join(" - ")}
            {/*{`${item.MaDeXuat} - ${item.TenDeXuat}`}*/}
          </a>
        </td>
        <td style={{width: '15%', textAlign: "center"}}>
          {!item.DuyetDeXuat && role.add ?
            <Button onClick={() => this.addThuyetMinh(item.DeXuatID)} type={'primary'}>Thêm</Button> : ""}
          {!item.DuyetDeXuat && item.ListThuyetMinh.length && roleDuyet.view ?
            <Button style={{marginLeft: 10}} onClick={() => this.duyetThuyetMinh(item.DeXuatID)}
                    type={'primary'}>Duyệt</Button> : ""}
        </td>
      </tr>
      {item.ListThuyetMinh && item.ListThuyetMinh.length > 0 && listExpandRow.includes(item.DeXuatID) ?
        item.ListThuyetMinh.map((child, indexChild) =>
          <tr>
            <td style={{width: '5%', textAlign: "center"}}>{indexChild + 1}</td>
            <td style={{width: '30%'}}>
              <a href={child.FileThuyetMinh.FileUrl} target={'_blank'}>{child.FileThuyetMinh.TenFileGoc}</a>
            </td>
            <td style={{width: '35%'}}>
              {this.renderTenCanBo(child.CanBoID, child.CoQuanID)}
            </td>
            <td style={{width: '15%', textAlign: "center"}}>{this.renderTrangThai(child.TrangThai)}</td>
            <td style={{width: '15%', textAlign: "center"}}>
              {!item.DuyetDeXuat && role.edit ?
                <Icon type={'edit'} onClick={() => this.editThuyetMinh(child.ThuyetMinhID)}/> : ""}
              {!item.DuyetDeXuat && role.delete ? <Icon style={{marginLeft: 10}} type={'delete'}
                                                        onClick={() => this.deleteThuyetMinh(child.ThuyetMinhID)}/> : ""}
            </td>
          </tr>
        )
        : ""}
      </tbody>
    )
  };

  viewChiTietDeXuat = (DeXuatID) => {
    const link = `thuyet-minh-de-tai/chi-tiet/${DeXuatID}`;
    this.props.history.push(link);
  };

  onExpandRow = (DeXuatID) => {
    const {listExpandRow} = this.state;
    if (listExpandRow.includes(DeXuatID)) {
      const index = listExpandRow.indexOf(DeXuatID);
      listExpandRow.splice(index, 1);
    } else {
      listExpandRow.push(DeXuatID);
    }
    this.setState({listExpandRow});
  };

  addExpandRow = (DeXuatID) => {
    const {listExpandRow} = this.state;
    if (!listExpandRow.includes(DeXuatID)) {
      listExpandRow.push(DeXuatID);
    }
    this.setState({listExpandRow});
  };

  submitModalDuyet = (ThuyetMinhID) => {
    const {DeXuatID} = this.state;
    const param = {ThuyetMinhID: ThuyetMinhID, DeXuatID: DeXuatID};
    this.setState({loading: true});
    api.DuyetThuyetMinh(param).then(response => {
      if (response.data.Status > 0) {
        message.destroy();
        message.success('Duyệt thuyết minh thành công');
        this.props.getList(this.state.filterData);
        this.setState({visibleModalDuyet: false, loading: false})
      } else {
        this.setState({loading: false});
        message.destroy();
        message.error(response.data.Message)
      }
    }).catch(error => {
      this.setState({loading: false});
      message.destroy();
      message.error(error.toString())
    });
  };

  closeModalDuyet = () => {
    this.setState({visibleModalDuyet: false});
  };

  render() {
    const {modalKey, visibleModalThuyetMinh, visibleModalDuyet, dataEdit, loading, filterData, actions, DanhSachDuyetThuyetMinh} = this.state;
    const {TableLoading, role, FileLimit, FileAllow, DanhSachThuyetMinh, DanhSachCanBo, DanhSachCapQuanLy} = this.props;
    const PageNumber = filterData.PageNumber ? parseInt(filterData.PageNumber) : 1;
    const PageSize = filterData.PageSize ? parseInt(filterData.PageSize) : getDefaultPageSize();
    return (
      <LayoutWrapper>
        <PageHeader>Thuyết minh đề tài</PageHeader>
        <Box>
          <BoxFilter>
            {/*<TreeSelectEllipsis*/}
            {/*  showSearch*/}
            {/*  data={DanhSachCapQuanLy}*/}
            {/*  defaultValue={filterData.CapQuanLy}*/}
            {/*  dropdownStyle={{maxHeight: 400, overflowX: 'hidden', maxWidth: 500}}*/}
            {/*  placeholder="Cấp quản lý"*/}
            {/*  allowClear*/}
            {/*  notFoundContent={"Không có dữ liệu"}*/}
            {/*  treeNodeFilterProp={'label'}*/}
            {/*  onChange={value => this.onSearch(value, 'CapQuanLy')}*/}
            {/*  style={{width: 300}}*/}
            {/*/>*/}
            <Search style={{width: 400}} defaultValue={filterData.Keyword} allowClear
                    placeholder={'Nhập tên đề xuất hoặc mã đề xuất cần tìm kiếm'}
                    onSearch={value => this.onSearch(value, 'Keyword')}/>
          </BoxFilter>
          <BoxTable loading={TableLoading}>
            <table className='table-head'>
              <tr>
                <th style={{width: '5%'}}>STT</th>
                <th style={{width: '30%'}}>Thuyết minh đề tài</th>
                <th style={{width: '35%'}}>Chủ nhiệm đề tài</th>
                <th style={{width: '15%'}}>Quyết định duyệt</th>
                <th style={{width: '15%'}}>Thao tác</th>
              </tr>
            </table>
            <div className='scroll'>
              <table className='table-scroll'>
                {this.renderData(DanhSachThuyetMinh)}
              </table>
            </div>
          </BoxTable>
          <div style={{fontStyle: 'italic', color: 'red', marginTop: 20}}>
            * Nhấn vào số thứ tự đề xuất để hiển thị danh sách thuyết minh
          </div>
        </Box>
        <ModalAddEdit visible={visibleModalThuyetMinh} actions={actions} key={modalKey} dataEdit={dataEdit}
                      DanhSachCanBo={DanhSachCanBo} onCreate={this.submitModalThuyetMinh} loading={loading}
                      onCancel={this.closeModalThuyetMinh} FileAllow={FileAllow} FileLimit={FileLimit}/>
        <ModalDuyetThuyetMinh DanhSachCanBo={DanhSachCanBo} visible={visibleModalDuyet} onOk={this.submitModalDuyet}
                              onCancel={this.closeModalDuyet} loading={loading} key={modalKey}
                              DanhSachDuyetThuyetMinh={DanhSachDuyetThuyetMinh}/>
      </LayoutWrapper>
    )
  }
}

function mapStateToProps(state) {
  return {
    ...state.ThuyetMinhDeTai
  };
}

export default connect(
  mapStateToProps,
  actions
)(ThuyetMinhDeTai);