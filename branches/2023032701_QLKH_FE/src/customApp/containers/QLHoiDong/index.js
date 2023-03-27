import React, {Component} from "react";
import {connect} from "react-redux";
import {Icon, Input, Modal, message, Checkbox, Spin, Pagination} from "antd";
import LayoutWrapper from "../../../components/utility/layoutWrapper";
import PageHeader from "../../../components/utility/pageHeader";
import Box from "../../../components/utility/box";
import BoxFilter from "../../../components/utility/boxFilter";
import BoxTable from '../../../components/utility/boxTable';
import PageAction from "../../../components/utility/pageAction";
import Button from "../../../components/uielements/button";
import actions from "../../redux/QLHoiDong/actions";
import {changeUrlFilter, getFilterData, getDefaultPageSize} from "../../../helpers/utility";
import queryString from 'query-string';
import api from "./config";
import "./style.css"
import {ModalAddEdit} from "./ModalAddEditHoiDong";
import {ModalDanhSachDeXuat} from "./ModalDanhSachDanhGia";

const {Search} = Input;

class QLHoiDong extends Component {
  constructor(props) {
    document.title = 'Quản lý hội đồng';
    super(props);
    const filterData = queryString.parse(this.props.location.search);
    this.formPrint = React.createRef();
    this.state = {
      selectedRowKeys: [],
      modalKey: 0,
      visibleModal: false,
      visibleModalDS: false,
      dataEdit: {},
      actions: "",
      loading: false,
      filterData: filterData,
      DanhSachDanhGia: [],
      HoiDongID: []
    }
  }

  componentDidMount() {
    this.props.getInitData(this.state.filterData);
  }

  changePage = (NewPageNumber, NewPageSize) => {
    let oldFilterData = {...this.state.filterData};
    //1: thay doi size (cu != moi; !cu (1:10) & moi != 10 ) -> pageNumber -> 1
    if (oldFilterData.PageSize) {
      if (oldFilterData.PageSize !== NewPageSize) {
        oldFilterData.PageNumber = 1;
        oldFilterData.PageSize = NewPageSize;
      } else {
        oldFilterData.PageNumber = NewPageNumber;
        oldFilterData.PageSize = NewPageSize;
      }
    } else {
      oldFilterData.PageNumber = NewPageNumber;
      oldFilterData.PageSize = NewPageSize;
    }
    //2: thay doi page: 1, size: 10 => pageNumber, pageSize -> undefine
    if ((oldFilterData.PageNumber === 1 && oldFilterData.PageSize === 10) || !oldFilterData.PageNumber) {
      delete oldFilterData.PageNumber;
      delete oldFilterData.PageSize;
    }
    //thay filter Data
    this.setState({filterData: oldFilterData}, () => {
      //lay lai list data . param = filterData
      changeUrlFilter(this.state.filterData); //change url
      this.props.getList(this.state.filterData); //get list
    })
  };

  onFilter = (value, property) => {
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

  deleteHoiDong = () => {
    const {selectedRowKeys} = this.state;
    this.setState({loading: false});
    Modal.confirm({
      title: 'Thông báo',
      content: 'Bạn chắc chắn muốn xóa hội đồng này không ?',
      okText: 'Có',
      cancelText: 'Không',
      onOk: () => {
        api.XoaHoiDong({HoiDongID: selectedRowKeys[0]})
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

  editHoiDong = () => {
    const {selectedRowKeys} = this.state;
    let {modalKey} = this.state;
    modalKey++;
    const HoiDongID = selectedRowKeys[0];
    this.setState({loading: true});
    api.ChiTietHoiDong({HoiDongID: HoiDongID})
      .then(response => {
        if (response.data.Status === 1 && response.data.Data) {
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

  addHoiDong = () => {
    let {modalKey} = this.state;
    modalKey++;
    this.setState({visibleModal: true, modalKey, actions: 'add', dataEdit: {}, loading: false});
  };

  closeModalAddEdit = () => {
    this.setState({visibleModal: false, selectedRowKeys: []});
  };

  renderTenCanBo = (CanBoID) => {
    if (!CanBoID) {
      return ""
    }
    const {DanhSachCanBo} = this.props;
    const canbo = DanhSachCanBo.filter(item => item.CanBoID === CanBoID);
    return canbo[0] ? canbo[0].TenCanBo : ""
  };

  submitModalAddEdit = (data) => {
    data.ThanhVienHoiDong = data.ThanhVienHoiDong.filter(item => item.CanBoID);
    this.setState({loading: true}, () => {
      api.SuaHoiDong(data)
        .then(response => {
          if (response.data.Status === 1) {
            message.destroy();
            message.success('Cập nhật danh sách hội đồng thành công');
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
      });
    });
  };

  changeSelected = (value, HoiDongID) => {
    value = value.target.checked;
    let {selectedRowKeys} = this.state;
    if (value) {
      selectedRowKeys = [];
      selectedRowKeys.push(HoiDongID);
    } else {
      // const index = selectedRowKeys.indexOf(HoiDongID);
      selectedRowKeys.splice(0, 1);
    }
    this.setState({selectedRowKeys});
  };

  changeCheckAll = (value) => {
    value = value.target.checked;
    const {DanhSachHoiDong} = this.props;
    let {selectedRowKeys} = this.state;
    if (value) {
      selectedRowKeys = [];
      DanhSachHoiDong.forEach(item => selectedRowKeys.push(item.HoiDongID))
    } else {
      selectedRowKeys = [];
    }
    this.setState({selectedRowKeys});
  };

  exportExcel = () => {
    let html, link, blob, url;
    let preHtml = `<html><head><meta charset='utf-8'></head><body>`;
    let postHtml = "</body></html>";
    html = preHtml + this.formPrint.current.innerHTML + postHtml;
    blob = new Blob(['\ufeff', html], {
      type: 'application/vnd.ms-excel'
    });
    url = URL.createObjectURL(blob);
    link = document.createElement('A');
    link.href = url;
    link.download = 'Danh sách hội đồng.xls';  // default name without extension
    document.body.appendChild(link);
    if (navigator.msSaveOrOpenBlob) navigator.msSaveOrOpenBlob(blob, 'Danh sách hội đồng.xls'); // IE10-11
    else link.click();  // other browsers
    document.body.removeChild(link);
  };

  showDanhSachDanhGia = () => {
    let {modalKey, selectedRowKeys} = this.state;
    modalKey++;
    // api.DanhSachDanhGia({HoiDongID: selectedRowKeys[0]})
    //   .then(response => {
    //     if (response.data.Status > 0) {
    //       const DanhSachDanhGia = response.data.Data;
    //       this.setState({visibleModalDS: true, modalKey, loading: false, DanhSachDanhGia});
    //     } else {
    //       message.destroy();
    //       message.error(response.data.Message)
    //     }
    //   }).catch(error => {
    //   message.destroy();
    //   message.error(error.toString())
    // });
    this.setState({visibleModalDS: true, modalKey, loading: false, HoiDongID: selectedRowKeys[0]});
  };

  closeModalDanhSach = () => {
    this.setState({visibleModalDS: false, selectedRowKeys: []});
  };

  render() {
    const {modalKey, visibleModal, dataEdit, selectedRowKeys, loading, filterData, actions, visibleModalDS, HoiDongID} = this.state;
    const {DanhSachHoiDong, TotalRow, TableLoading, role, DanhSachCanBo, DanhSachCapQuanLy} = this.props;
    // console.log(262,DanhSachHoiDong);
    const PageNumber = filterData.PageNumber ? parseInt(filterData.PageNumber) : 1;
    const PageSize = filterData.PageSize ? parseInt(filterData.PageSize) : getDefaultPageSize();
    const border = {border: 'solid 0.5pt #999', verticalAlign: 'middle', padding: 5};
    return (
      <LayoutWrapper>
        <div style={{display: "flex", justifyContent:"space-between", padding:'8px 16px'}}>
          <PageHeader>Quản lý hội đồng tư vấn và xét duyệt đề tài</PageHeader>
          <PageAction>
            {role.view ? <Button type={'primary'} style={{marginRight: 5}} onClick={this.showDanhSachDanhGia}
                                disabled={selectedRowKeys.length !== 1}>DS đánh giá</Button> : ""}
            {role.delete ? <Button type={'primary'} style={{marginRight: 5}} onClick={this.deleteHoiDong}
                                  disabled={selectedRowKeys.length !== 1}>Xóa</Button> : ""}
            {role.edit ? <Button type={'primary'} style={{marginRight: 5}} onClick={this.editHoiDong}
                                disabled={selectedRowKeys.length !== 1}>Sửa</Button> : ""}
            {role.view ?
              <Button type={'primary'} style={{marginRight: 5}} onClick={this.exportExcel}>Xuất excel</Button> : ""}
            {role.add ? <Button type={'primary'} onClick={this.addHoiDong}>Thêm hội đồng</Button> : ""}
          </PageAction>
        </div>
        <Box>
          <BoxFilter>
            <Search placeholder={'Nhập tên cán bộ cần tìm kiếm'} style={{width: 300, marginLeft: 10, marginBottom: 20}}
                    allowClear
                    onSearch={value => this.onFilter(value, 'Keyword')} defaultValue={filterData.Keyword}/>
          </BoxFilter>
          <table className={'table-scroll-head'}>
            <thead>
            <tr>
              <th style={{width: '5%'}}>
                {/*<Checkbox onChange={this.changeCheckAll}*/}
                {/*          checked={DanhSachHoiDong.length > 0 && selectedRowKeys.length === DanhSachHoiDong.length}/>*/}
              </th>
              <th style={{width: '5%'}}>STT</th>
              <th style={{width: '25%'}}>Tên hội đồng</th>
              <th style={{width: '25%'}}>Thành viên hội đồng</th>
              <th style={{width: '20%'}}>Đơn vị công tác</th>
              <th style={{width: '20%'}}>Vai trò</th>
            </tr>
            </thead>
          </table>
          <div className={'scrollable-table'}>
            {TableLoading ? <div className={'loading-div'}><Spin style={{position: "absolute"}}/></div> : ""}
            <table className={'table-scroll'}>
              {DanhSachHoiDong.map((item, index) => (
                <tbody>
                <tr>
                  <td rowSpan={item.ThanhVienHoiDong.length} style={{textAlign: 'center', width: '5%'}}>
                    <Checkbox checked={selectedRowKeys.includes(item.HoiDongID)}
                              onChange={value => this.changeSelected(value, item.HoiDongID)}/>
                  </td>
                  <td rowSpan={item.ThanhVienHoiDong.length > 0 ? item.ThanhVienHoiDong.length : 1}
                      style={{textAlign: 'center', width: '5%'}}>
                    {index + 1}
                  </td>
                  <td rowSpan={item.ThanhVienHoiDong.length} style={{width: '25%'}}
                      colSpan={item.ThanhVienHoiDong.length > 0 ? 1 : 4}>
                    {item.TenHoiDong}
                  </td>
                  {item.ThanhVienHoiDong.length > 0 ? <td style={{width: '25%'}}>
                    {item.ThanhVienHoiDong[0] ? this.renderTenCanBo(item.ThanhVienHoiDong[0].CanBoID) : ""}
                  </td> : ""}
                  {item.ThanhVienHoiDong.length > 0 ? <td style={{width: '20%'}}>
                    {item.ThanhVienHoiDong[0] ? item.ThanhVienHoiDong[0].DonViCongTac : ""}
                  </td> : ""}
                  {item.ThanhVienHoiDong.length > 0 ? <td style={{width: '20%'}}>
                    {item.ThanhVienHoiDong[0] ? item.ThanhVienHoiDong[0].VaiTro : ""}
                  </td> : ""}
                </tr>
                {item.ThanhVienHoiDong.length > 1 ? item.ThanhVienHoiDong.map((thanhvien, index) => {
                  return index === 0 ? "" : <tr>
                    <td>
                      {this.renderTenCanBo(thanhvien.CanBoID)}
                    </td>
                    <td>
                      {thanhvien.DonViCongTac}
                    </td>
                    <td>
                      {thanhvien.VaiTro}
                    </td>
                  </tr>
                }) : ""}
                </tbody>
              ))}
            </table>
          </div>
          <div style={{textAlign: "right", marginTop: 20}}>
            <Pagination onChange={this.changePage}
                        onShowSizeChange={(page, size) => this.onFilter(size, "PageSize")}
                        current={PageNumber}
                        total={TotalRow}
                        showTotal={(total, range) => `${range[0]}-${range[1]} trên ${total} bản`}
                        defaultPageSize={PageSize}
                        size={PageSize}
                        showSizeChanger={true}
            />
          </div>
          <ModalAddEdit actions={actions} loading={loading} DanhSachCanBo={DanhSachCanBo}
                        onCancel={this.closeModalAddEdit} dataEdit={dataEdit}
                        visible={visibleModal} key={modalKey} onCreate={this.submitModalAddEdit}/>
          <ModalDanhSachDeXuat DanhSachCanBo={DanhSachCanBo} visible={visibleModalDS} key={modalKey} loading={loading}
                               onCancel={this.closeModalDanhSach} HoiDongID={HoiDongID}
                               DanhSachCapQuanLy={DanhSachCapQuanLy}/>
        </Box>
        <div style={{display: "none"}} ref={this.formPrint}>
          <div style={{fontFamily: "Times New Roman"}}>
            <table>
              <tr>
                <td colSpan={5} rowSpan={3} style={{textAlign: 'center', verticalAlign: 'middle'}}>
                  <b style={{fontSize: 20}}>Danh sách hội đồng</b>
                </td>
              </tr>
              <tr/>
              <tr/>
              <tr>
                <td style={{...border, textAlign: 'center', width: 50, fontWeight: 'bold'}}>STT</td>
                <td style={{...border, textAlign: 'center', width: 250, fontWeight: 'bold'}}>Tên hội đồng</td>
                <td style={{...border, textAlign: 'center', width: 200, fontWeight: 'bold'}}>Thành viên hội đồng</td>
                <td style={{...border, textAlign: 'center', width: 200, fontWeight: 'bold'}}>Đơn vị công tác</td>
                <td style={{...border, textAlign: 'center', width: 150, fontWeight: 'bold'}}>Vai trò</td>
              </tr>
              {DanhSachHoiDong.map((item, index) => (
                <tbody>
                <tr>
                  <td rowSpan={item.ThanhVienHoiDong.length}
                      style={{textAlign: 'center', verticalAlign: 'middle', ...border}}>
                    {index + 1}
                  </td>
                  <td rowSpan={item.ThanhVienHoiDong.length > 0 ? item.ThanhVienHoiDong.length : 1}
                      colSpan={item.ThanhVienHoiDong.length > 0 ? 1 : 4}
                      style={{...border, verticalAlign: 'middle', textAlign: 'left'}}>
                    {item.TenHoiDong}
                  </td>
                  {item.ThanhVienHoiDong.length > 0 ? <td style={{...border, textAlign: 'left'}}>
                    {item.ThanhVienHoiDong[0] ? this.renderTenCanBo(item.ThanhVienHoiDong[0].CanBoID) : ""}
                  </td> : ""}
                  {item.ThanhVienHoiDong.length > 0 ? <td style={{...border, textAlign: 'left'}}>
                    {item.ThanhVienHoiDong[0] ? item.ThanhVienHoiDong[0].DonViCongTac : ""}
                  </td> : ""}
                  {item.ThanhVienHoiDong.length > 0 ? <td style={{...border, textAlign: 'left'}}>
                    {item.ThanhVienHoiDong[0] ? item.ThanhVienHoiDong[0].VaiTro : ""}
                  </td> : ""}
                </tr>
                {item.ThanhVienHoiDong.length > 1 ? item.ThanhVienHoiDong.map((thanhvien, index) => {
                  return index === 0 ? "" : <tr>
                    <td style={{...border, textAlign: 'left'}}>
                      {this.renderTenCanBo(thanhvien.CanBoID)}
                    </td>
                    <td style={{...border, textAlign: 'left'}}>
                      {thanhvien.DonViCongTac}
                    </td>
                    <td style={{...border, textAlign: 'left'}}>
                      {thanhvien.VaiTro}
                    </td>
                  </tr>
                }) : ""}
                </tbody>
              ))}
            </table>
          </div>
        </div>
      </LayoutWrapper>
    )
  }
}

function mapStateToProps(state) {
  return {
    ...state.QLHoiDong
  };
}

export default connect(
  mapStateToProps,
  actions
)(QLHoiDong);