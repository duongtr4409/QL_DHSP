import React, {Component} from "react";
import {connect} from "react-redux";
import {Input, Modal, message, Card, Avatar, Spin, Pagination, Row, Col, Icon} from "antd";
import LayoutWrapper from "../../../components/utility/layoutWrapper";
import PageHeader from "../../../components/utility/pageHeader";
import Box from "../../../components/utility/box";
import BoxFilter from "../../../components/utility/boxFilter";
import PageAction from "../../../components/utility/pageAction";
import Button from "../../../components/uielements/button";
import actions from "../../redux/QLNhaKhoaHoc/actions";
import moment from "moment";
import defaultAvatar from "../../../image/defaultAvatar.jpeg";
import defaultAvatar_Female from "../../../image/defaultAvatar_female.jpg";
import {changeUrlFilter, getFilterData, getDefaultPageSize} from "../../../helpers/utility";
import queryString from "query-string";
import api, {apiUrl} from "./config";
import "./style.css";
import Select, {Option} from "../../../components/uielements/select";
import {ModalAddEdit} from "./ModalAddEditNhaKhoaHoc";
import {formDataCaller} from "../../../helpers/formDataCaller";
import Link from "react-router-dom/Link";

const {Search} = Input;
const {Meta} = Card;

class QLNhaKhoaHoc extends Component {
  constructor(props) {
    document.title = "Quản lý nhà khoa học, chuyên gia";
    super(props);
    const filterData = queryString.parse(this.props.location.search);
    this.formPrint = React.createRef();
    this.state = {
      selectedRowKeys: [],
      modalKey: 0,
      visibleModal: false,
      dataEdit: {},
      actions: "",
      loading: false,
      filterData: filterData,
    };
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
    });
  };

  onFilter = (value, property) => {
    //get filter data
    let oldFilterData = this.state.filterData;
    let onFilter = {value, property};
    let filterData = getFilterData(oldFilterData, onFilter, null);
    //get filter data
    this.setState(
      {
        filterData,
      },
      () => {
        // console.log(this.state.filterData);
        changeUrlFilter(this.state.filterData); //change url
        this.props.getList(this.state.filterData); //get list
      }
    );
  };

  addNhaKhoaHoc = () => {
    let {modalKey} = this.state;
    modalKey++;
    this.setState({visibleModal: true, modalKey, actions: "add", loading: false});
  };

  closeModalAddEdit = () => {
    this.setState({visibleModal: false, selectedRowKeys: []});
  };

  submitModalAddEdit = (data, AnhDaiDien, FileLyLich) => {
    this.setState({loading: true});
    const form = new FormData();
    data.NgaySinh = data.NgaySinh && data.NgaySinh !== "" ? moment(data.NgaySinh).format("YYYY-MM-DD") : "";
    form.append("ThongTinNhaKhoaHoc", JSON.stringify(data));
    form.append("AnhDaiDien", AnhDaiDien.FileData);
    FileLyLich.forEach((item) => form.append("files", item.FileData));
    formDataCaller(apiUrl.themnhakhoahoc, form)
      .then((response) => {
        if (response.data.Status > 0) {
          message.destroy();
          message.success("Thêm mới nhà khoa học thành công");
          this.setState({visibleModal: false, loading: false});
          this.props.getList(this.state.filterData);
        } else {
          this.setState({loading: false});
          message.destroy();
          message.error(response.data.Message);
        }
      })
      .catch((error) => {
        this.setState({loading: false});
        message.destroy();
        message.error(error.toString());
      });
  };

  renderInfoNhaKhoaHoc = (data) => {
    return (
      <div className={"card-info"}>
        {this.renderTenChucVu(data.ChucDanhKhoaHoc)} <b className={"upper-case"}>{data.TenCanBo}</b>
        <br/>
        Chức vụ: {this.renderTenChucDanh(data.ChucDanhHanhChinh)}
        <br/>
        Đơn vị công tác: {data.CoQuanCongTac}
        <br/>
        Điện thoại: {data.DienThoai}
        <br/>
        Email: {data.Email}
      </div>
    );
  };

  renderTenChucDanh = (ListID, type = 1) => {
    if (!ListID) {
      return "";
    }
    const {DanhSachChucDanh} = this.props;
    let DanhSachTenChucDanh = [];
    ListID.forEach((Id) => {
      const chucdanh = DanhSachChucDanh.filter((item) => item.Id === Id);
      if (chucdanh && chucdanh.length > 0) {
        DanhSachTenChucDanh.push(chucdanh[0].Name);
      }
    });
    return DanhSachTenChucDanh.join(", ");
  };

  renderTenChucVu = (ListID, type = 1) => {
    if (!ListID) {
      return "";
    }
    const {DanhSachHocVi} = this.props;
    let DanhSachTenChucVu = [];
    ListID.forEach((Id) => {
      const chucvu = DanhSachHocVi.filter((item) => item.Id === Id);
      if (chucvu && chucvu.length > 0) {
        DanhSachTenChucVu.push(chucvu[0].Name);
      }
    });
    return DanhSachTenChucVu.join(", ");
  };

  exportExcel = () => {
    let html, link, blob, url;
    let preHtml = `<html><head><meta charset='utf-8'></head><body>`;
    let postHtml = "</body></html>";
    html = preHtml + this.formPrint.current.innerHTML + postHtml;
    blob = new Blob(["\ufeff", html], {
      type: "application/vnd.ms-excel",
    });
    url = URL.createObjectURL(blob);
    link = document.createElement("A");
    link.href = url;
    link.download = "Danh sách nhà Khoa Học.xls"; // default name without extension
    document.body.appendChild(link);
    if (navigator.msSaveOrOpenBlob) navigator.msSaveOrOpenBlob(blob, "Danh sách nhà Khoa Học.xls");
    // IE10-11
    else link.click(); // other browsers
    document.body.removeChild(link);
  };

  render() {
    const {DanhSachNhaKhoaHoc, FileLimit, TotalRow, TableLoading, role, roleLyLich, DanhSachChucDanh, DanhSachHocVi, FileAllow} = this.props;
    const {modalKey, visibleModal, loading, filterData} = this.state;
    const PageNumber = filterData.PageNumber ? parseInt(filterData.PageNumber) : 1;
    const PageSize = filterData.PageSize ? parseInt(filterData.PageSize) : 12;
    let listWord = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    listWord = listWord.split("");
    // console.log(DanhSachNhaKhoaHoc);
    const QuyenQuanLy = filterData.QuyenQuanLy ? filterData.QuyenQuanLy : "1";
    //style for table print
    const tblstyle = {
      th: {
        textAlign: "center",
        fontWeight: "bold",
        border: "solid 0.5pt #999",
        verticalAlign: "middle",
        padding: 5,
      },
      td: {
        border: "solid 0.5pt #999",
        padding: 5,
        verticalAlign: "middle",
      },
    };
    return (
      <LayoutWrapper>
        {TableLoading ? (
          <div className={"loading-div"}>
            <Spin/>
          </div>
        ) : (
          ""
        )}
        <PageHeader>Quản lý nhà khoa học, chuyên gia</PageHeader>
        <PageAction>
          {role.view ? (
            <Button type={"primary"} onClick={this.exportExcel}>
              Xuất excel
            </Button>
          ) : (
            ""
          )}
          {role.add ? (
            <Button type={"primary"} onClick={this.addNhaKhoaHoc}>
              Thêm mới
            </Button>
          ) : (
            ""
          )}
        </PageAction>
        <Box>
          <BoxFilter>
            <Select style={{width: 200}} placeholder={"Chọn quyền quản lý"} showSearch showArrow={false}
                    optionFilterProp={"label"} defaultValue={QuyenQuanLy}
                    onChange={(value) => this.onFilter(value, "QuyenQuanLy")}>
              <Option value={"1"}>Trong trường</Option>
              <Option value={"2"}>Ngoài trường</Option>
            </Select>
            <Search placeholder={"Nhập họ tên cán bộ cần tìm kiếm"} style={{width: 300}} allowClear
                    onSearch={(value) => this.onFilter(value, "Keyword")} defaultValue={filterData.Keyword}/>
          </BoxFilter>
          <div className={"div-search-by-word"}>
            <a onClick={() => this.onFilter(null, "Alphabet")}>All</a>
            {listWord.map((word) => (
              <a onClick={() => this.onFilter(word, "Alphabet")}
                 className={filterData.Alphabet === word ? "word-chosen" : ""}>
                {word}
              </a>
            ))}
          </div>
          <Card bordered={false} className={"box-card"}>
            {DanhSachNhaKhoaHoc && DanhSachNhaKhoaHoc.length ? DanhSachNhaKhoaHoc.map((item) => {
              const GioiTinh = item.GioiTinh !== null ? item.GioiTinh : item.GioiTinhStr !== "Nam" ? 0 : 1;
              return (
                <Card.Grid hoverable={false} bordered={false} style={!role.view ? {cursor: "unset"} : {}}>
                  <Link disabled={!role.view}
                        to={`chi-tiet-nha-khoa-hoc?ref=ql-nha-khoa-hoc?QuyenQuanLy=${QuyenQuanLy}&CanBoID=${item.CanBoID}&CoQuanID=${item.CoQuanID}`}>
                    <Meta avatar={<Avatar
                      src={item.AnhHoSo && item.AnhHoSo !== "" ? item.AnhHoSo : GioiTinh ? defaultAvatar : defaultAvatar_Female}
                      style={{width: 120, height: 120}}/>} description={this.renderInfoNhaKhoaHoc(item)}/>
                  </Link>
                </Card.Grid>
              );
            }) : ""}
          </Card>
          {TotalRow > PageSize ? (
            <div style={{textAlign: "right"}}>
              <Pagination onChange={this.changePage} onShowSizeChange={(page, size) => this.onFilter(size, "PageSize")}
                          current={PageNumber} total={TotalRow}
                          showTotal={(total, range) => `${range[0]}-${range[1]} trên ${total} bản`}
                          defaultPageSize={PageSize} size={PageSize}/>
            </div>
          ) : (
            ""
          )}
        </Box>
        <ModalAddEdit key={modalKey} visible={visibleModal} loading={loading} onCreate={this.submitModalAddEdit}
                      onCancel={this.closeModalAddEdit} FileLimit={FileLimit} DanhSachChucDanh={DanhSachChucDanh}
                      DanhSachHocVi={DanhSachHocVi} FileAllow={FileAllow}/>
        <div ref={this.formPrint} id={"form-print"} style={{display: "none"}}>
          <table style={{fontFamily: "Times New Roman"}}>
            <tr>
              <th style={{...tblstyle.th, fontSize: 20, border: "none"}} colSpan={9}>
                DANH SÁCH NHÀ KHOA HỌC, CHUYÊN GIA
              </th>
            </tr>
            <tr/>
            <tr>
              <th style={{...tblstyle.th, width: 50}}>STT</th>
              <th style={{...tblstyle.th, width: 150}}>Mã cán bộ</th>
              <th style={{...tblstyle.th, width: 200}}>Tên cán bộ</th>
              <th style={{...tblstyle.th, width: 100}}>Ngày sinh</th>
              <th style={{...tblstyle.th, width: 150}}>Chức danh khoa học</th>
              <th style={{...tblstyle.th, width: 250}}>Chức danh hành chính</th>
              <th style={{...tblstyle.th, width: 200}}>Cơ quan công tác</th>
              <th style={{...tblstyle.th, width: 150}}>Điện thoại</th>
              <th style={{...tblstyle.th, width: 200}}>Email</th>
            </tr>
            {DanhSachNhaKhoaHoc && DanhSachNhaKhoaHoc.length ? DanhSachNhaKhoaHoc.map((item, index) => (
              <tr>
                <td style={{...tblstyle.td, textAlign: "center"}}>{index + 1}</td>
                <td style={{...tblstyle.td, textAlign: "center"}}>{item.MaCB}</td>
                <td style={{...tblstyle.td}}>{item.TenCanBo}</td>
                <td style={{
                  ...tblstyle.td,
                  textAlign: "center"
                }}>{item.NgaySinh ? moment(item.NgaySinh).format("DD/MM/YYYY") : ""}</td>
                <td style={{...tblstyle.td}}>{this.renderTenChucVu(item.ChucDanhKhoaHoc, 2)}</td>
                <td style={{...tblstyle.td}}>{this.renderTenChucDanh(item.ChucDanhHanhChinh, 2)}</td>
                <td style={{...tblstyle.td}}>{item.CoQuanCongTac}</td>
                <td style={{...tblstyle.td, textAlign: "center"}}>{item.DienThoai ? `${item.DienThoai}'` : ""}</td>
                <td style={{...tblstyle.td}}>{item.Email}</td>
              </tr>
            )) : ""}
          </table>
        </div>
      </LayoutWrapper>
    );
  }
}

function mapStateToProps(state) {
  return {
    ...state.QLNhaKhoaHoc,
  };
}

export default connect(mapStateToProps, actions)(QLNhaKhoaHoc);
