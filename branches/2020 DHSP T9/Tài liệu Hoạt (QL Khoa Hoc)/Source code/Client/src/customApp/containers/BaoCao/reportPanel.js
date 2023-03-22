import React from "react";
import {
  Icon,
  Button,
  Input,
  Divider,
  Tooltip,
  message,
  Avatar,
  Spin,
  Table,
  Radio,
  Modal,
  Empty,
  DatePicker,
  Select
} from "antd";
import {DatePicker as DatePicker4} from 'antd4'
import lodash from "lodash";
import {TreeSelectWithApi, SelectWithApi} from "../../components/index";
import {ValidatorForm} from "react-form-validator-core";
import api from "./config";
import moment from 'moment'

export default class Panel extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      filterData: {
        NamThucHien: [],
        TenNhiemVu: "",
        MaNhiemVu: "",
        // LoaiHinhNghienCuu: null,
        // LinhVucNghienCuu: null,
        // LinhVucKinhTeXaHoi: null,
        // CapQuanLy: null,
        // CanBoID: null,
        // MaCanBo: null,
        // HoTen: null,
        // GioiTinhStr: null,
        // DanToc: null,
        // HocHamHocVi: null,
        // DonViCongTac: null,
        // ChucVu: null,
      },

      // canBoData: {

      // },
    };
  }

  handleCreateReport = () => {
    const {filterData} = this.state;

    const newFilter = lodash.clone(filterData);
    delete newFilter.NamThucHien, (newFilter.NamBatDau = filterData.NamThucHien[0] ? filterData.NamThucHien[0].format("YYYY") : null);
    newFilter.NamKetThuc = filterData.NamThucHien[1] ? filterData.NamThucHien[1].format("YYYY") : null;
    if (this.props.reportType === 5) {
      const currentYear = moment().format("YYYY");
      const filter5 = {
        NamBatDau: filterData.NamBatDau ? filterData.NamBatDau : currentYear,
        NamKetThuc: filterData.NamKetThuc ? filterData.NamKetThuc : currentYear,
      };
      this.props.onCreateReport(filter5);
    }
    else {
      this.props.onCreateReport(newFilter);
    }
  };

  render() {
    const {filterData} = this.state;
    return (
      <ValidatorForm ref="form" onSubmit={() => {
      }}>
        {this.props.reportType === 5 ?
          <div className="my-2 row">
            <div className="col-8 col-lg-6">
              <div className="row my-2 align-items-center">
                <div className="col-lg-2 col-6 my-1">Năm bắt đầu</div>
                <div className="col-lg-4 col-6 my-1">
                  <DatePicker4 format="YYYY"
                               value={this.state.filterData.NamBatDau ? moment(this.state.filterData.NamBatDau, 'YYYY') : null}
                               picker="year" placeholder="Chọn năm"
                               onChange={(value) => {
                                 const {filterData} = this.state;
                                 filterData.NamBatDau = value ? value.format('YYYY') : value;
                                 this.setState({filterData});
                               }}/>
                </div>
                <div className="col-lg-2 col-6 my-1">Năm kết thúc</div>
                <div className="col-lg-4 col-6 my-1">
                  <DatePicker4 format="YYYY"
                               value={this.state.filterData.NamKetThuc ? moment(this.state.filterData.NamKetThuc, 'YYYY') : null}
                               picker="year" placeholder="Chọn năm"
                               onChange={(value) => {
                                 const {filterData} = this.state;
                                 filterData.NamKetThuc = value ? value.format('YYYY') : value;
                                 this.setState({filterData});
                               }}/>
                </div>
              </div>
            </div>
          </div> : ""}
        {this.props.reportType !== 5 ?
          <div className="row my-2">
            <div className="col-12 col-lg-6">
              <Icon type="menu"/> <span className="font-weight-bold ml-2">LỌC THEO NHIỆM VỤ KHOA HỌC</span>
              <div className="row my-2 align-items-center">
                <div className="col-lg-3 col-12 my-1">Năm thực hiện</div>
                <div className="col-lg-9 col-12 my-1">
                  <DatePicker.RangePicker
                    className="w-100"
                    format="YYYY"
                    placeholder={["", ""]}
                    mode={["year", "year"]}
                    value={filterData.NamThucHien}
                    onPanelChange={(value) => {
                      const {filterData} = this.state;
                      filterData.NamThucHien = value;
                      this.setState({filterData});
                    }}
                    onChange={(value) => {
                      const {filterData} = this.state;
                      filterData.NamThucHien = value;
                      this.setState({filterData});
                    }}
                  />
                </div>
                <div className="col-lg-3 col-12 my-1">Tên nhiệm vụ</div>
                <div className="col-lg-9 col-12  my-1">
                  <Input
                    onChange={(event) => {
                      const {filterData} = this.state;
                      filterData.TenNhiemVu = event.target.value;
                      this.setState({filterData});
                    }}
                  />
                </div>
                <div className="col-lg-3 col-12 my-1">Mã nhiệm vụ</div>
                <div className="col-lg-9 col-12  my-1">
                  <Input
                    onChange={(event) => {
                      const {filterData} = this.state;
                      filterData.MaNhiemVu = event.target.value;
                      this.setState({filterData});
                    }}
                  />
                </div>
                <div className="col-lg-3 col-12 my-1">Loại hình nghiên cứu</div>
                <div className="col-lg-9 col-12  my-1">
                  <SelectWithApi
                    allowClear
                    placeholder="Chọn loại hình nghiên cứu"
                    apiConfig={{
                      api: api.danhSachLoaiHinhNghienCuu,
                      valueField: "Id",
                      nameField: "Name",
                    }}
                    value={filterData.LoaiHinhNghienCuu}
                    onChange={(value) => {
                      const {filterData} = this.state;
                      filterData.LoaiHinhNghienCuu = value;
                      this.setState({filterData});
                    }}
                  />
                </div>
                <div className="col-lg-3 col-12 my-1">Lĩnh vực nghiên cứu</div>
                <div className="col-lg-9 col-12  my-1">
                  <TreeSelectWithApi
                    apiConfig={{
                      api: api.danhSachCayLinhVuc,
                      valueField: "ID",
                      nameField: "Name",
                      codeField: "Code",
                      filter: {
                        Type: 1,
                        status: true,
                      },
                    }}
                    placeholder="Chọn lĩnh vực"
                    onChange={(value) => {
                      const {filterData} = this.state;
                      filterData.LinhVucNghienCuu = value;
                      this.setState({filterData});
                    }}
                    value={filterData.LinhVucNghienCuu}
                  />
                </div>
                <div className="col-lg-3 col-12 my-1">Lĩnh vực kinh tế - xã hội</div>
                <div className="col-lg-9 col-12  my-1">
                  <TreeSelectWithApi
                    apiConfig={{
                      api: api.danhSachCayLinhVuc,
                      valueField: "ID",
                      nameField: "Name",
                      codeField: "Code",
                      filter: {
                        Type: 2,
                        status: true,
                      },
                    }}
                    placeholder="Chọn lĩnh vực"
                    onChange={(value) => {
                      const {filterData} = this.state;
                      filterData.LinhVucKinhTeXaHoi = value;
                      this.setState({filterData});
                    }}
                    value={filterData.LinhVucKinhTeXaHoi}
                  />
                </div>
                <div className="col-lg-3 col-12 my-1">Loại nhiệm vụ/ Cấp quản lý</div>
                <div className="col-lg-9 col-12  my-1">
                  <TreeSelectWithApi
                    apiConfig={{
                      api: api.danhSachCapDeTai,
                      valueField: "ID",
                      nameField: "Name",
                      filter: {Status: true},
                    }}
                    placeholder="Cấp quản lý"
                    onChange={(value) => {
                      const {filterData} = this.state;
                      filterData.CapQuanLy = value;
                      this.setState({filterData});
                    }}
                    value={filterData.CapQuanLy}
                  />
                </div>
              </div>
            </div>
            <div className="col-12 col-lg-6">
              <Icon type="menu"/> <span className="font-weight-bold ml-2">LỌC THEO CHỦ NHIỆM ĐỀ TÀI</span>
              <div className="row my-2 align-items-center">
                <div className="col-lg-3 col-12 my-1">Mã cán bộ</div>
                <div className="col-lg-9 col-12  my-1">
                  <Input
                    value={filterData.MaCanBo}
                    onChange={(event) => {
                      const {filterData} = this.state;
                      filterData.MaCanBo = event.target.value;
                      this.setState({filterData});
                    }}
                  />
                </div>
                <div className="col-lg-3 col-12 my-1">Họ và tên</div>
                <div className="col-lg-9 col-12  my-1">
                  <Input
                    value={filterData.HoTen}
                    onChange={(event) => {
                      const {filterData} = this.state;
                      filterData.HoTen = event.target.value;
                      this.setState({filterData});
                    }}
                  />
                </div>
                <div className="col-lg-3 col-12 my-1">Năm sinh</div>
                <div className="col-lg-9 col-12  my-1">
                  <DatePicker4 picker="year" placeholder="Chọn năm sinh" className="w-100"
                               value={filterData.NamSinh ? moment(filterData.NamSinh, 'YYYY') : null}
                               onChange={(value) => {
                                 const {filterData} = this.state;
                                 filterData.NamSinh = value ? value.format('YYYY') : value;
                                 this.setState({filterData})
                               }}/>
                </div>
                <div className="col-lg-3 col-12 my-1">Giới tính</div>
                <div className="col-lg-9 col-12  my-1">
                  <Select
                    placeholder="Chọn giới tính"
                    allowClear
                    value={filterData.GioiTinhStr}
                    onChange={(value) => {
                      const {filterData} = this.state;
                      filterData.GioiTinhStr = value;
                      this.setState({filterData});
                    }}
                  >
                    <Select.Option value="Nam">Nam</Select.Option>
                    <Select.Option value="Nữ">Nữ</Select.Option>
                  </Select>
                </div>
                <div className="col-lg-3 col-12 my-1">Học hàm, Học vị</div>
                <div className="col-lg-9 col-12  my-1">
                  <SelectWithApi
                    placeholder="Chọn học hàm, học vị"
                    apiConfig={{
                      api: api.danhSachHocHamHocVi,
                      valueField: "Id",
                      nameField: "Name",
                    }}
                    value={filterData.HocHamHocVi}
                    onChange={(value) => {
                      const {filterData} = this.state;
                      filterData.HocHamHocVi = value;
                      this.setState({filterData});
                    }}
                  />
                </div>
                <div className="col-lg-3 col-12 my-1">Đơn vị công tác</div>
                <div className="col-lg-9 col-12  my-1">
                  <SelectWithApi
                    placeholder="Chọn đơn vị công tác"
                    apiConfig={{
                      api: api.danhSachDonViCongTac,
                      valueField: "Id",
                      nameField: "Name",
                      filter: {
                        type: 0,
                      },
                    }}
                    value={filterData.DonViCongTac}
                    onChange={(value) => {
                      const {filterData} = this.state;
                      filterData.DonViCongTac = value;
                      this.setState({filterData});
                    }}
                  />
                </div>
                <div className="col-lg-3 col-12 my-1">Chức vụ</div>
                <div className="col-lg-9 col-12  my-1">
                  <SelectWithApi
                    placeholder="Chọn chức vụ"
                    apiConfig={{
                      api: api.danhSachChucVu,
                      valueField: "Id",
                      nameField: "Name",
                    }}
                    value={filterData.ChucVu}
                    onChange={(value) => {
                      const {filterData} = this.state;
                      filterData.ChucVu = value;
                      this.setState({filterData});
                    }}
                  />
                </div>
              </div>
            </div>
          </div> : ""
        }
        <div className="row my-2">
          <div className="col-12 text-center">
            <Button type="primary" onClick={this.handleCreateReport} loading={this.props.loading}>
              Tạo báo cáo
            </Button>
            <Button disabled={!this.props.loaded} type="primary" className="mx-2" onClick={this.props.onPrint}>
              In báo cáo
            </Button>
            <Button type="primary" onClick={this.props.onExport} disabled={!this.props.loaded}>
              Xuất Excel
            </Button>
          </div>
        </div>
      </ValidatorForm>
    );
  }
}
