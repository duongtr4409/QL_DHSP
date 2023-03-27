import React, {Component} from 'react';
import {Modal, Input, message, Icon, Popover, TreeSelect, Checkbox, Button} from 'antd';
import BoxTable from '../../../components/utility/boxTable';
import BoxFilter from "../../../components/utility/boxFilter";
import Select, {Option} from "../../../components/uielements/select";
import api from './config';

const {Search} = Input;

class ModalDanhSachDeXuat extends Component {
  constructor(props) {
    super(props);
    this.formPrint = React.createRef();
    this.state = {
      filterData: {
        HoiDongID: null,
        CapQuanLy: null,
        CanBoID: null,
        Keyword: ""
      },
      DanhSachDeXuatID: [],
      DanhSachDeXuat: [],
      loading: false,
      loadingData: false
    };
  }

  onOk = () => {
    const {HoiDongID} = this.props;
    const {DanhSachDeXuatID} = this.state;
    const param = [];
    DanhSachDeXuatID.forEach(DeXuatID => param.push({HoiDongID: HoiDongID, DeXuatID: DeXuatID}));
    if (param.length === 0) {
      param.push({HoiDongID: HoiDongID});
    }
    this.setState({loading: true});
    api.SaveDanhSachDanhGia(param)
      .then(response => {
        if (response.data.Status > 0) {
          message.destroy();
          message.success('Lưu danh sách đánh giá đề xuất thành công');
          this.setState({loading: false});
          this.props.onCancel();
        } else {
          message.destroy();
          message.error(response.data.Message);
          this.setState({loading: false});
        }
      }).catch(error => {
      this.setState({loading: false});
      message.destroy();
      message.error(error.toString());
    });
  };

  componentDidMount() {
    const {HoiDongID} = this.props;
    const {filterData} = this.state;
    filterData.HoiDongID = HoiDongID;

    this.setState({filterData}, () => {
      if (HoiDongID > 0) {
        this.getDanhSachDeXuatFilter();
      }
    });
  }

  onFilter = (value, keyword) => {
    const {filterData} = this.state;
    // if (value) {
    filterData[keyword] = value;
    // }
    this.setState({filterData}, () => {
      //Get Danh Sách Đề xuất
      this.getDanhSachDeXuatFilter();
    })
  };

  getDanhSachDeXuatFilter = () => {
    const {filterData, DanhSachDeXuatID} = this.state;
    this.setState({loadingData: true});
    api.DanhSachDanhGia(filterData).then(response => {
      // console.log(response);
      if (response.data.Status > 0) {
        const DanhSachDeXuat = response.data.Data;
        DanhSachDeXuat.forEach(item => {
          if (item.DanhGia) {
            DanhSachDeXuatID.push(item.DeXuatID);
          }
        });
        this.setState({DanhSachDeXuat, DanhSachDeXuatID, loadingData: false});
      } else {
        this.setState({loadingData: false});
        message.destroy();
        message.warning(response.data.Message);
      }
    }).catch(error => {
      this.setState({loadingData: false});
      message.destroy();
      message.warning(error.toString());
    });
  };

  removeHtml = (html) => {
    const htmlReg = /(<([^>]+)>)/ig;
    return html.replace(htmlReg, " ");
  };

  exportExcel = () => {
    const {DanhSachDeXuatID} = this.state;
    if (DanhSachDeXuatID.length === 0) {
      message.destroy();
      message.info('Chưa chọn danh sách đánh giá');
      return;
    }
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
    link.download = 'Danh sách đánh giá đề xuất.xls';  // default name without extension
    document.body.appendChild(link);
    if (navigator.msSaveOrOpenBlob) navigator.msSaveOrOpenBlob(blob, 'Danh sách đánh giá đề xuất.xls'); // IE10-11
    else link.click();  // other browsers
    document.body.removeChild(link);
  };

  changeChecked = (value, DeXuatID) => {
    const {DanhSachDeXuatID} = this.state;
    if (value) {
      DanhSachDeXuatID.push(DeXuatID);
    } else {
      const index = DanhSachDeXuatID.indexOf(DeXuatID);
      DanhSachDeXuatID.splice(index, 1);
    }
    this.setState({DanhSachDeXuatID})
  };

  renderDanhSachPrint = () => {
    const border = {border: 'solid 0.5pt #999', verticalAlign: 'middle', padding: 5};
    const {DanhSachDeXuat, DanhSachDeXuatID} = this.state;
    const DanhSachPrint = DanhSachDeXuat.filter(item => {
      if (DanhSachDeXuatID.includes(item.DeXuatID)) {
        return true;
      }
    });
    return DanhSachPrint.map((item, index) => (
      <tr>
        <td style={{...border, textAlign: 'center'}}>{index + 1}</td>
        <td style={{...border, textAlign: 'center'}}>{item.MaDeXuat}</td>
        <td style={{...border, textAlign: 'left'}}>{item.TenDeXuat}</td>
        <td style={{...border, textAlign: 'left'}}>{this.removeHtml(item.TenLinhVucNghienCuu)}</td>
        <td style={{...border, textAlign: 'left'}}>{this.removeHtml(item.TenLinhVucKinhTeXaHoi)}</td>
        <td style={{...border, textAlign: 'left'}}>{item.TenCapQuanLy}</td>
        <td style={{...border, textAlign: 'left'}}>{item.TenNguoiDeXuat}</td>
      </tr>
    ));
  };

  render() {
    const {visible, onCancel, DanhSachCanBo, DanhSachCapQuanLy} = this.props;
    const {DanhSachDeXuatID, DanhSachDeXuat, loading, loadingData} = this.state;
    const border = {border: 'solid 0.5pt #999', verticalAlign: 'middle', padding: 5};
    const column = [
      {
        title: 'STT',
        align: 'center',
        width: '5%',
        render: (text, record, index) => (
          <span>{index + 1}</span>
        )
      },
      {
        title: 'Mã đề xuất',
        dataIndex: 'MaDeXuat',
        width: '10%',
        align: 'center'
      },
      {
        title: 'Tên đề xuất',
        dataIndex: 'TenDeXuat',
        width: '20%',
      },
      {
        title: 'Lĩnh vực nghiên cứu',
        dataIndex: 'TenLinhVucNghienCuu',
        width: '20%',
        render: (text, record) => (
          this.removeHtml(record.TenLinhVucNghienCuu)
        )
      },
      {
        title: 'Lĩnh vực KT - XH',
        width: '15%',
        render: (text, record) => (
          this.removeHtml(record.TenLinhVucKinhTeXaHoi)
        )
      },
      {
        title: 'Cấp quản lý',
        dataIndex: 'TenCapQuanLy',
        width: '10%'
      },
      {
        title: 'Người đề xuất',
        dataIndex: 'TenNguoiDeXuat',
        width: '10%',
      },
      {
        title: 'Chọn đánh giá',
        width: '10%',
        align: 'center',
        render: (text, record, index) => (
          <Checkbox checked={DanhSachDeXuatID.includes(record.DeXuatID)}
                    onChange={value => this.changeChecked(value.target.checked, record.DeXuatID)}/>
        )
      }
    ];

    return (
      <Modal
        title={"Danh sách đề xuất xét duyệt"}
        width={"80%"}
        visible={visible}
        onCancel={onCancel}
        footer={[
          <Button type="primary" onClick={this.onOk} loading={loading} disabled={loading}>Lưu</Button>,
          <Button type="primary" onClick={this.exportExcel}>Xuất excel</Button>,
        ]}
      >
        <BoxFilter>
          <TreeSelect
            showSearch
            treeData={DanhSachCapQuanLy}
            defaultValue={undefined}
            style={{width: 200}}
            dropdownStyle={{maxHeight: 400, overflow: 'auto', maxWidth: 400}}
            placeholder="Chọn cấp quản lý"
            allowClear
            treeDefaultExpandAll
            onChange={value => this.onFilter(value, "CapQuanLy")}
            notFoundContent={"Không có dữ liệu"}
            treeNodeFilterProp={'label'}
          />
          <Select placeholder={'Chọn người đề xuất'} allowClear showSearch style={{width: 200}}
                  notFoundContent={'Không có dữ liệu'} onChange={value => this.onFilter(value, 'CanBoID')}>
            {DanhSachCanBo.map(item => (
              <Option value={item.CanBoID} label={item.TenCanBo}>{item.TenCanBo}</Option>
            ))}
          </Select>
          <Search placeholder={'Nhập mã hoặc tên đề tài cần tìm kiếm'} allowClear style={{width: 300}}
                  onSearch={value => this.onFilter(value, 'Keyword')}/>
        </BoxFilter>
        <BoxTable columns={column} dataSource={DanhSachDeXuat} pagination={false} scroll={{y: 400}}
                  loading={loadingData}/>
        <div ref={this.formPrint} style={{display: 'none'}}>
          <div style={{fontFamily: "Times New Roman"}}>
            <table>
              <tr>
                <td colSpan={7} rowSpan={3} style={{textAlign: 'center', verticalAlign: 'middle'}}>
                  <b style={{fontSize: 20}}>Danh sách đánh giá đề xuất</b>
                </td>
              </tr>
              <tr/>
              <tr/>
              <tr>
                <td style={{...border, textAlign: 'center', width: 50, fontWeight: 'bold'}}>STT</td>
                <td style={{...border, textAlign: 'center', width: 150, fontWeight: 'bold'}}>Mã đề xuất</td>
                <td style={{...border, textAlign: 'center', width: 200, fontWeight: 'bold'}}>Tên đề xuất</td>
                <td style={{...border, textAlign: 'center', width: 200, fontWeight: 'bold'}}>Lĩnh vực nghiên cứu</td>
                <td style={{...border, textAlign: 'center', width: 200, fontWeight: 'bold'}}>Lĩnh vực kinh tế xã hội
                </td>
                <td style={{...border, textAlign: 'center', width: 200, fontWeight: 'bold'}}>Cấp quản lý</td>
                <td style={{...border, textAlign: 'center', width: 200, fontWeight: 'bold'}}>Người đề xuất</td>
              </tr>
              {this.renderDanhSachPrint()}
            </table>
          </div>
        </div>
      </Modal>
    );
  }
}

export {
  ModalDanhSachDeXuat
}