import React, {Component} from "react";
import {connect} from "react-redux";
import {Icon, Input, Modal, message} from "antd";
import LayoutWrapper from "../../../components/utility/layoutWrapper";
import PageHeader from "../../../components/utility/pageHeader";
import Box from "../../../components/utility/box";
import BoxFilter from "../../../components/utility/boxFilter";
import BoxTable from '../../../components/utility/boxTable';
import actions from "../../redux/DonViNghienCuu/actions";
import {changeUrlFilter, getFilterData, getDefaultPageSize} from "../../../helpers/utility";
import queryString from 'query-string';
import {ModalDanhSachCanBo} from "./modalDanhSachCanBo";
import {ModalDanhSachDeXuat} from "./modalDanhSachDeXuat";
import api from './config';

const {Search} = Input;

class DonViNghienCuu extends Component {
  constructor(props) {
    document.title = 'Đơn vị nghiên cứu';
    super(props);
    const filterData = queryString.parse(this.props.location.search);
    this.state = {
      modalKey: 0,
      visibleModalNhanSu: false,
      visibleModalDeXuat: false,
      filterData: filterData,
      DanhSachCanBoDonVi: [],
      loadCanBo: false,
      CoQuanIDFilter: 0
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

  closeModalDanhSachCanBo = () => {
    this.setState({visibleModalNhanSu: false});
  };

  closeModalDanhSachDeXuat = () => {
    this.setState({visibleModalDeXuat: false, CoQuanIDFilter: 0});
  };

  openModalNhanSu = (CoQuanID) => {
    let {modalKey} = this.state;
    modalKey++;
    this.setState({loadCanBo: true});
    api.DanhSachNhanSuDonVi({CoQuanID: CoQuanID}).then(response => {
      if (response.data.Status > 0) {
        const DanhSachCanBoDonVi = response.data.Data;
        this.setState({visibleModalNhanSu: true, DanhSachCanBoDonVi, loadCanBo: false, modalKey})
      } else {
        this.setState({loadCanBo: false});
        message.destroy();
        message.error(response.data.Message)
      }
    }).catch(error => {
      this.setState({loadCanBo: false});
      message.destroy();
      message.error(error.toString())
    });
  };

  openModalDeXuat = (CoQuanID, SoLuong) => {
    if (SoLuong <= 0) {
      message.destroy();
      message.warning('Không có đề xuất');
      return;
    }
    let {modalKey} = this.state;
    modalKey++;
    this.setState({loadCanBo: true});
    api.DanhSachNhanSuDonVi({CoQuanID: CoQuanID}).then(response => {
      if (response.data.Status > 0) {
        const DanhSachCanBoDonVi = response.data.Data;
        this.setState({
          visibleModalDeXuat: true,
          DanhSachCanBoDonVi,
          loadCanBo: false,
          modalKey,
          CoQuanIDFilter: CoQuanID
        })
      } else {
        this.setState({loadCanBo: false});
        message.destroy();
        message.error(response.data.Message)
      }
    }).catch(error => {
      this.setState({loadCanBo: false});
      message.destroy();
      message.error(error.toString())
    });
    // this.setState({visibleModalDeXuat: true, modalKey, CoQuanIDFilter: CoQuanID});
  };

  render() {
    const {modalKey, visibleModalNhanSu, visibleModalDeXuat, filterData, DanhSachCanBoDonVi, loadCanBo, CoQuanIDFilter} = this.state;
    const {DanhSachDonViNghienCuu, TotalRow, TableLoading, DanhSachLinhVuc, DanhSachCapQuanLy} = this.props;
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
        title: 'Tên đơn vị',
        dataIndex: 'TenCoQuan',
        width: '15%',
      },
      {
        title: 'Địa chỉ',
        dataIndex: 'DiaChi',
        width: '10%'
      },
      {
        title: 'Điện thoại',
        dataIndex: 'DienThoai',
        width: '10%',
        align: 'center'
      },
      {
        title: 'Email',
        dataIndex: 'Email',
        width: '10%',
      },
      {
        title: 'SL bài báo khoa học',
        dataIndex: 'SoLuongBaiBao',
        width: '10%',
        align: "center"
      },
      {
        title: 'SL nhiệm vụ NC đang thực hiện',
        dataIndex: 'SoLuongDeTai',
        width: '10%',
        align: "center"
      },
      {
        title: 'SL sách xuất bản',
        dataIndex: 'SoLuongSach',
        width: '10%',
        align: "center"
      },
      {
        title: 'Nhân sự đơn vị',
        width: '10%',
        align: 'center',
        render: (text, record) => (
          <a onClick={() => this.openModalNhanSu(record.CoQuanID)}>
            <ins>{record.NhanSuDonVi}</ins>
          </a>
        )
      },
      {
        title: 'SL đề xuất đã gửi',
        width: '10%',
        align: 'center',
        render: (text, record) => (
          <a onClick={() => this.openModalDeXuat(record.CoQuanID, record.SoLuongDeXuatDaGui)}>
            <ins>{record.SoLuongDeXuatDaGui}</ins>
          </a>
        )
      }
    ];
    return (
      <LayoutWrapper>
        <PageHeader>Quản lý đơn vị nghiên cứu trong trường</PageHeader>
        <Box>
          <BoxFilter>
            <Search placeholder={'Nhập tên đơn vị cần tìm kiếm'} style={{width: 300, marginLeft: 10, marginBottom: 20}}
                    allowClear
                    onSearch={value => this.onSearch(value, 'Keyword')} defaultValue={filterData.Keyword}/>
            {/*<Search placeholder={'Nhập tên đơn vị cần tìm kiếm'} style={{width: 300}}*/}
            {/*        allowClear*/}
            {/*        onSearch={value => this.onSearch(value, 'Keyword')} defaultValue={filterData.Keyword}/>*/}
          </BoxFilter>
          <BoxTable
            columns={column}
            rowKey={'DonViID'}
            dataSource={DanhSachDonViNghienCuu}
            pagination={{
              showSizeChanger: true, //show text: pageSize/page
              showTotal: (total, range) => `Từ ${range[0]} đến ${range[1]} trên ${total}`,
              total: TotalRow, //test 100
              current: PageNumber, //current page
              pageSize: PageSize,
            }}
            loading={TableLoading || loadCanBo}
            onChange={this.onTableChange}
          />
        </Box>
        <ModalDanhSachCanBo onCancel={this.closeModalDanhSachCanBo} visible={visibleModalNhanSu}
                            DanhSachCanBoDonVi={DanhSachCanBoDonVi} key={modalKey}/>
        <ModalDanhSachDeXuat onCancel={this.closeModalDanhSachDeXuat} visible={visibleModalDeXuat}
                             DanhSachCanBoDonVi={DanhSachCanBoDonVi} key={modalKey} CoQuanID={CoQuanIDFilter}
                             DanhSachLinhVuc={DanhSachLinhVuc}/>
      </LayoutWrapper>
    )
  }
}

function mapStateToProps(state) {
  return {
    ...state.DonViNghienCuu
  };
}

export default connect(
  mapStateToProps,
  actions
)(DonViNghienCuu);