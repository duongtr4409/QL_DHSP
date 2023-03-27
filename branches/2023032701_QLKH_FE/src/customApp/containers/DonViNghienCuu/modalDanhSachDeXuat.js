import React, {Component} from 'react';
import {Modal, Input, message, Icon, Popover, TreeSelect} from 'antd';
import BoxTable from '../../../components/utility/boxTable';
import BoxFilter from "../../../components/utility/boxFilter";
import Select, {Option} from "../../../components/uielements/select";
import api from './config';
import apiDeXuat from "../QLDeXuat/config";
import {GoTreeSelect, withAPI} from "../../components";
import {ValidatorForm} from "react-form-validator-core";

const TreeSelectWithApi = withAPI(GoTreeSelect);

const {Search} = Input;

class ModalDanhSachDeXuat extends Component {
  constructor(props) {
    super(props);
    this.state = {
      filterData: {
        CoQuanID: null,
        LinhVucID: null,
        CanBoID: null,
        Keyword: ""
      },
      DanhSachDeXuat: []
    };
  }

  componentDidMount() {
    const {CoQuanID} = this.props;
    const {filterData} = this.state;
    filterData.CoQuanID = CoQuanID;

    this.setState({filterData}, () => {
      if (CoQuanID > 0) {
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
    const {filterData} = this.state;
    api.DanhSachDeXuat(filterData).then(response => {
      // console.log(response);
      if (response.data.Status > 0) {
        this.setState({DanhSachDeXuat: response.data.Data});
      } else {
        message.destroy();
        message.warning(response.data.Message);
      }
    }).catch(error => {
      message.destroy();
      message.warning(error.toString());
    });
  };

  removeHtml = (html) => {
    const htmlReg = /(<([^>]+)>)/ig;
    return html.replace(htmlReg, " ");
  };

  renderContentDownload = (fileDinhKem) => {
    return <div style={{maxHeight: 300, overflowY: 'auto'}}>
      {fileDinhKem.map(item => (
        <div style={{padding: 5, margin: 10, borderBottom: 'solid 1px #d6d6d6'}}>
          <a href={item.FileUrl} className={'link-hover'}>{item.TenFileGoc}</a>
        </div>
      ))}
    </div>
  };

  render() {
    const {visible, onCancel, DanhSachCanBoDonVi, DanhSachLinhVuc} = this.props;
    const {DanhSachDeXuat, filterData} = this.state;
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
        width: '15%',
      },
      {
        title: 'Lĩnh vực nghiên cứu',
        dataIndex: 'TenLinhVucNghienCuu',
        width: '15%',
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
        title: 'Trạng thái',
        dataIndex: 'TenTrangThai',
        width: '10%',
        align: 'center'
      },
      {
        title: 'File đính kèm',
        width: '10%',
        align: 'center',
        render: (text, record) => (
          record.FileDinhKem.length > 0 ? <Popover content={this.renderContentDownload(record.FileDinhKem)}>
            <Icon type="cloud-download"
                  style={{fontSize: 30, color: '#4482FF'}}/>
          </Popover> : ""
        )
      },
    ];

    return (
      <Modal
        title={"Danh sách đề xuất đã gửi"}
        width={"80%"}
        visible={visible}
        onCancel={onCancel}
        footer={null}
      >
        <BoxFilter>
          {/*<TreeSelect*/}
          {/*  showSearch*/}
          {/*  treeData={DanhSachLinhVuc}*/}
          {/*  defaultValue={undefined}*/}
          {/*  style={{width: 200}}*/}
          {/*  dropdownStyle={{maxHeight: 400, maxWidth: 500, overflowX: 'hidden'}}*/}
          {/*  placeholder="Chọn lĩnh vực"*/}
          {/*  allowClear*/}
          {/*  treeDefaultExpandAll*/}
          {/*  onChange={value => this.onFilter(value, "LinhVucID")}*/}
          {/*  notFoundContent={"Không có dữ liệu"}*/}
          {/*  treeNodeFilterProp={'label'}*/}
          {/*/>*/}
          <ValidatorForm onSubmit={() => {
          }}>
            <div className="row mb-2 mr-1">
              <div className="col-6 col-lg-2">
                <TreeSelectWithApi
                  apiConfig={{
                    api: apiDeXuat.danhSachCayLinhVuc,
                    valueField: "ID",
                    nameField: "Name",
                    filter: {
                      Type: 1,
                      status: true,
                    },
                  }}
                  onChange={value => this.onFilter(value, "LinhVucID")}
                  allowClear
                  placeholder={'Chọn lĩnh vực'}
                  value={filterData.LinhVucID}
                />
              </div>
              <div className="col-6 col-lg-2">
                <Select placeholder={'Chọn cán bộ'} allowClear showSearch
                        notFoundContent={'Không có dữ liệu'} onChange={value => this.onFilter(value, 'CanBoID')}>
                  {DanhSachCanBoDonVi.map(item => (
                    <Option value={item.CanBoID} label={item.TenCanBo}>{item.TenCanBo}</Option>
                  ))}
                </Select>
              </div>
              <div className="col-6 col-lg-3">
                <Search placeholder={'Nhập mã hoặc tên đề tài cần tìm kiếm'} allowClear
                        onSearch={value => this.onFilter(value, 'Keyword')}/>
              </div>
            </div>
          </ValidatorForm>
        </BoxFilter>
        <BoxTable columns={column} dataSource={DanhSachDeXuat} pagination={false} scroll={{y: 400}}/>
      </Modal>
    );
  }
}

export {
  ModalDanhSachDeXuat
}