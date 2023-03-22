/**
 *
 * GoInput
 *
 */

import React from "react";
import { Modal, Icon, Button, Input, Divider, Upload, PageHeader, Tooltip, message, Avatar, Spin, Radio } from "antd";
import { Form } from "antd4";
import { GoInput, GoSelect, withAPI, SelectWithApi, TreeSelectWithApi, GoDatePicker, GoEditor, RCSelectWithApi, GoSelect4 } from "../../components/index";
import { Select } from "antd4";
import { ValidatorForm } from "react-form-validator-core";
import { ModalAddEdit } from "./ThemCanBoModal";
import api from "./config";
import moment from "moment";
import lodash from "lodash";
import { checkFilesSize, checkFileType } from "../../../helpers/utility";
const { TextArea } = Input;
// import PropTypes from 'prop-types';
// import styled from 'styled-components';

/* eslint-disable react/prefer-stateless-function */
class KQNghienCuuModal extends React.Component {
  modal = null;
  loadingSearch = false;
  constructor(props) {
    super(props);
    this.submitBtn = React.createRef();
    this.state = {
      confirmLoading: false,
      MaCanBo: null,
      CanBoData: {},
      dataDeXuat: {
        MaDeTai: "",
        TenDeTai: "",
        LoaiHinhNghienCuu: null,
        LinhVucNghienCuu: null,
        LinhVucKinhTeXaHoi: null,
        CapQuanLy: 1,
        NhiemVu: null,
        CoQuanChuQuan: "",
        DonViQuanLyKhoaHoc: "",
        ChuNhiemDeTaiID: null,
        CoQuanChuNhiemID: null,
        VaiTroChuNhiemID: null,
        NamBatDau: null,
        NamKetThuc: null,
        KinhPhiDHSP: 0,
        NguonKhac: 0,
        MucTieu: "",
        CacNoiDungChinh: [
          {
            TenNoiDung: "",
            MoTa: "",
          },
        ],
        SanPhamDangKy: "",
        KhaNangUngDung: "",
        ThanhVienNghienCuu: [
          { CanBoID: null, VaiTro: null, LaCanBoTrongTruong: null },
          { CanBoID: null, VaiTro: null, LaCanBoTrongTruong: null },
        ],
        DonViPhoiHop: "",
        FileDinhKem: [{ TenFileGoc: "", NoiDung: "", files: [] }],
      },
      danhSachVaiTro: [],
      searching: false,
      danhSachCanBo: [],
      loadDanhSachCanBo: false,
      loaichunhiem: "trongtruong",
    };
  }
  componentDidMount() {
    api.danhSachAllCanBo({ PageSize: 999999, TrangThaiID: 0 }).then((res) => {
      // console.log(res);
      if (!res.data || res.data.Status !== 1) {
        message.error("Lấy danh sách cán bộ thất bại");
      } else {
        this.setState({ danhSachCanBo: res.data.Data, loadedDanhSachCanBo: true });
      }
    });
  }
  handleCancel = () => {
    this.props.onClose();
  };
  handleCloseModal = (value = null, CanBoID = null) => {
    let { dataDeXuat, danhSachCanBo } = this.state;
    if (value === "ThemNKH") {
      api.danhSachAllCanBo({ PageSize: 999999, TrangThaiID: 0 }).then((res) => {
        if (!res.data || res.data.Status !== 1) {
          message.error("Lấy danh sách cán bộ thất bại");
        } else {
          dataDeXuat.ThanhVienNghienCuu[this.rowCanBo].CanBoID = CanBoID;
          dataDeXuat.ThanhVienNghienCuu[this.rowCanBo].LaCanBoTrongTruong = 0;
          this.setState({ danhSachCanBo: res.data.Data, dataDeXuat, loadedDanhSachCanBo: true });
        }
      });
    }
    if (value === "ThemChuNhiem") {
      api.danhSachAllCanBo({ PageSize: 999999 }).then((res) => {
        if (!res.data || res.data.Status !== 1) {
          message.error("Lấy danh sách cán bộ thất bại");
        } else {
          dataDeXuat.ChuNhiemDeTaiID = CanBoID;
          danhSachCanBo = res.data.Data;
          const canbo = danhSachCanBo.find((d) => d.CanBoID === CanBoID && d.LaCanBoTrongTruong === false);
          dataDeXuat.CoQuanChuNhiemID = canbo.CoQuanID;
          this.setState({ danhSachCanBo: res.data.Data, dataDeXuat, loadedDanhSachCanBo: true, CanBoData: canbo });
        }
      });
    }
    this.modal.destroy();
  };
  handleOk = () => {
    this.submitBtn.current.click();
  };

  handleSubmit = () => {
    if (this.state.CanBoData == {}) {
      message.warning("Vui lòng chọn chủ nhiệm đề tài hợp lệ");
      return;
    }
    // console.log(this.state.dataDeXuat);
    this.setState({ confirmLoading: true });
    const { dataDeXuat } = this.state;
    const formData = new FormData();
    let files = [];
    const newData = lodash.cloneDeep(dataDeXuat);
    newData.ThanhVienNghienCuu = [];
    dataDeXuat.ThanhVienNghienCuu.forEach((item) => {
      if (item.CanBoID || item.VaiTro) {
        newData.ThanhVienNghienCuu.push(item);
      }
    });
    newData.FileDinhKem.forEach((element, index) => {
      files = [...files, ...element.files];
      delete newData.FileDinhKem[index].files;
    });

    files.forEach((element, index) => {
      formData.append("files", files[index]);
    });
    formData.append("ThongTinDeTai", JSON.stringify(newData));

    api.themDeTai(formData).then((res) => {
      if (!res || !res.data || res.data.Status !== 1) {
        this.setState({ confirmLoading: false });
        message.error(`${res && res.data ? res.data.Message : "Lỗi hệ thống"}`);
      } else {
        message.success("Thêm mới thành công");
        this.props.onClose();
      }
    });
  };

  handleInputChange = (name) => (event) => {
    const { dataDeXuat } = this.state;
    dataDeXuat[name] = event && event.target ? event.target.value : event;
    this.setState({
      dataDeXuat,
    });
  };
  handleChangeMaCanBo = (event) => {
    let { MaCanBo } = this.state;
    MaCanBo = event.target.value;
    // this.loadingSearch = true;
    this.setState({ MaCanBo, CanBoData: {} });
    if (this.timeout) {
      clearTimeout(this.timeout);
    }
    this.timeout = setTimeout(() => {
      this.handleSearchByCode();
    }, 2000);
    // if (!event.target.value) {
    //   this.onFilter();
    // }
  };
  handleSearchByCode = () => {
    if (this.timeout) {
      clearTimeout(this.timeout);
    }
    if (!this.state.MaCanBo) {
      message.error("Vui lòng nhập mã cán bộ");
      return;
    }
    this.setState({ searching: true });
    api.thongTinCanBoTheoMa({ MaCanBo: this.state.MaCanBo }).then((res) => {
      if (res) {
        if (res.data && res.data.Data) {
          this.state.dataDeXuat.ChuNhiemDeTaiID = res.data.Data.CanBoID;
          this.state.dataDeXuat.CoQuanChuNhiemID = res.data.Data.CoQuanID;
          this.setState({ CanBoData: res.data.Data, searching: false });
        } else {
          this.setState({ searching: false });
          message.error(`${res.data ? "Không tìm thấy cán bộ" : "Lỗi hệ thống"}`);
        }
      } else {
        this.setState({ searching: false });
        message.error(`Không thể tìm kiếm cán bộ`);
      }
    });
  };
  handleSelectNhiemVu = (value, option) => {
    const { dataDeXuat } = this.state;
    if (!value) {
      dataDeXuat.NhiemVu = null;
      dataDeXuat.VaiTroChuNhiemID = null;
      dataDeXuat.ThanhVienNghienCuu.forEach((item, index) => {
        dataDeXuat.ThanhVienNghienCuu[index].VaiTro = null;
      });
      this.setState({ dataDeXuat, danhSachVaiTro: [] });
      return;
    }
    if (!option || option.MappingId === 0 || !option.VaiTroChuNhiemID) {
      message.warning("Nhiệm vụ được chọn chưa có trong hệ thống giờ giảng.");
    } else {
      dataDeXuat.NhiemVu = value;
      dataDeXuat.VaiTroChuNhiemID = option.VaiTroChuNhiemID;

      this.state.dataDeXuat = dataDeXuat;
      api.danhSachVaiTroTheoNhiemVu({ CategoryId: option.MappingId }).then((res) => {
        if (res) {
          // format data core trả về phù hợp với select
          const danhSachVaiTro = res.data.Data.filter((d) => d.Id !== option.VaiTroChuNhiemID).map((item) => ({ ...item, ...{ value: item.Id, text: item.Name } }));
          if (res.data.Status === 1) {
            this.setState({ danhSachVaiTro });
          } else {
            message.error(`${res.message || "Lấy danh sách vai trò nhiệm vụ thất bại"}`);
          }
        } else {
          message.error("Lấy danh sách vai trò nhiệm vụ thất bại");
        }
      });
    }
  };
  handleSelectChange = (name) => (value) => {
    const { dataDeXuat } = this.state;
    dataDeXuat[name] = value;
    this.setState({
      dataDeXuat,
    });
  };

  handleSelectMember = (index) => (value, option) => {
    const { dataDeXuat } = this.state;
    dataDeXuat.ThanhVienNghienCuu[index].CanBoID = value;
    dataDeXuat.ThanhVienNghienCuu[index].CoQuanID = option.CoQuanID;
    this.setState({
      dataDeXuat,
    });
  };
  handleAddMember = () => {
    const { dataDeXuat } = this.state;
    dataDeXuat.ThanhVienNghienCuu.push({ CanBoID: null, VaiTro: null, CoQuanID: null });
    this.setState({ dataDeXuat });
  };
  handleMinusMember = () => {
    const { dataDeXuat } = this.state;
    dataDeXuat.ThanhVienNghienCuu.splice(dataDeXuat.ThanhVienNghienCuu.length - 1, 1);
    this.setState({ dataDeXuat });
  };

  handleAddPeople = (laChuNhiem = false) => {
    this.modal = Modal.confirm({
      icon: <i />,
      content: <ModalAddEdit onClose={this.handleCloseModal} laChuNhiem={laChuNhiem}></ModalAddEdit>,
    });
  };
  handleAddFileField = () => {
    const { dataDeXuat } = this.state;
    dataDeXuat.FileDinhKem.push({ TenFileGoc: "", NoiDung: "", files: [] });
    this.setState({ dataDeXuat });
  };
  onEditorDone = (name) => (value) => {
    const { dataDeXuat } = this.state;
    dataDeXuat[name] = value;
    this.setState({ dataDeXuat });
  };

  renderChonChuNhiemNgoaiTruong = () => {
    const { CanBoData, danhSachCanBo } = this.state;
    const { role } = this.props;
    return (
      <div className="row">
        <div className="col-lg-6 col-12 my-1  ">
          <div className="row align-items-center">
            <div className="col-6 col-lg-4 ">
              Họ và tên <span className="text-danger">*</span>
            </div>
            <div className="col-6 col-lg-8 ">
              <GoSelect4
                allowClear
                showSearch
                withTextToInput
                value={this.state.dataDeXuat.ChuNhiemDeTaiID}
                dropdownRender={(menu) => (
                  <div>
                    {menu}
                    <Divider style={{ margin: "4px 0" }} />
                    <div style={{ display: "flex", flexWrap: "nowrap", padding: 8 }} onMouseDown={(e) => e.preventDefault()}>
                      <span
                        className="text-primary pointer"
                        onClick={() => {
                          this.handleAddPeople(true);
                        }}
                      >
                        <Icon type="plus" /> Thêm cán bộ
                      </span>
                    </div>
                  </div>
                )}
                validators={["required"]}
                errorMessages={["Nội dung bắt buộc"]}
                onChange={(value) => {
                  const { dataDeXuat } = this.state;
                  const canbo = danhSachCanBo.find((d) => d.CanBoID === value && d.LaCanBoTrongTruong === false);

                  dataDeXuat.ChuNhiemDeTaiID = canbo.CanBoID;
                  dataDeXuat.CoQuanChuNhiemID = canbo.CoQuanID;
                  this.setState({ dataDeXuat, CanBoData: canbo });
                  // console.log(value, option);
                }}
              >
                {danhSachCanBo
                  .filter((d) => d.LaCanBoTrongTruong === false)
                  .map((item) => (
                    <Select.Option key={item.CanBoID} value={item.CanBoID} label={item.TenCanBo}>
                      <div className="row align-items-center">
                        <div className="col-2">{item.AnhHoSo ? <Avatar src={item.AnhHoSo} /> : <Avatar icon={<Icon type="user" />} />}</div>
                        <div className="col-10">
                          <p className="mb-0 ">
                            <b>{item.TenCanBo}</b>
                          </p>
                          <Tooltip title={item.TenChucVu}>
                            <p className="mb-0  text-truncate">Chức vụ: {item.TenChucVu}</p>
                          </Tooltip>

                          <p className="mb-0 ">Đơn vị công tác: {item.TenDonViCongTac}</p>
                        </div>
                      </div>
                    </Select.Option>
                  ))}
              </GoSelect4>
              {/* <Input disabled value={CanBoData.TenCanBo}></Input> */}
            </div>
          </div>
        </div>
        <div className="col-lg-6 col-12 my-1  ">
          <div className="row align-items-center">
            <div className="col-6 col-lg-4 ">Học hàm, học vị</div>
            <div className="col-6 col-lg-8 ">
              <Input disabled value={CanBoData.TenHocHamHocVi}></Input>
            </div>
          </div>
        </div>
        {/* <div className="col-lg-6 col-12 my-1  ">
          <div className="row align-items-center">
            <div className="col-6 col-lg-4 ">Chức danh </div>
            <div className="col-6 col-lg-8 ">
              <Input disabled value={CanBoData.TenChucDanh}></Input>
            </div>
          </div>
        </div> */}
        <div className="col-lg-6 col-12 my-1  ">
          <div className="row align-items-center">
            <div className="col-6 col-lg-4 ">Năm sinh</div>
            <div className="col-6 col-lg-8 ">
              <Input disabled value={CanBoData.NgaySinh ? moment(CanBoData.NgaySinh).format("DD/MM/YYYY") : null}></Input>
            </div>
          </div>
        </div>
        <div className="col-lg-6 col-12 my-1  ">
          <div className="row align-items-center">
            <div className="col-6 col-lg-4 ">Giới tính</div>
            <div className="col-6 col-lg-8 ">
              <Input disabled value={CanBoData.GioiTinhStr}></Input>
            </div>
          </div>
        </div>
        <div className="col-lg-6 col-12 my-1  ">
          <div className="row align-items-center">
            <div className="col-6 col-lg-4 ">Địa chỉ</div>
            <div className="col-6 col-lg-8 ">
              <Input disabled value={CanBoData.DiaChi}></Input>
            </div>
          </div>
        </div>
        <div className="col-lg-6 col-12 my-1  ">
          <div className="row align-items-center">
            <div className="col-6 col-lg-4 ">Email</div>
            <div className="col-6 col-lg-8 ">
              <Input disabled value={CanBoData.Email}></Input>
            </div>
          </div>
        </div>
        <div className="col-lg-6 col-12 my-1  ">
          <div className="row align-items-center">
            <div className="col-6 col-lg-4 ">Điện thoại di động</div>
            <div className="col-6 col-lg-8 ">
              <Input disabled value={CanBoData.DienThoai}></Input>
            </div>
          </div>
        </div>
        <div className="col-lg-6 col-12 my-1  ">
          <div className="row align-items-center">
            <div className="col-6 col-lg-4 ">Fax</div>
            <div className="col-6 col-lg-8 ">
              <Input disabled value={CanBoData.Fax}></Input>
            </div>
          </div>
        </div>
      </div>
    );
  };
  renderChonChuNhiemTrongTruong = () => {
    const { CanBoData } = this.state;
    const { role } = this.props;
    return (
      <div className="row">
        <div className="col-lg-6 col-12 my-1  ">
          <div className="row align-items-center">
            <div className="col-6 col-lg-4 ">
              Mã cán bộ <span className="text-danger">*</span>
            </div>
            <div className="col-6 col-lg-8 ">
              <GoInput
                onKeyDown={(event) => {
                  if (event.key === "Enter") {
                    event.preventDefault();
                    this.handleSearchByCode();
                  }
                }}
                withTextToInput
                validators={["required"]}
                errorMessages={["Nội dung bắt buộc"]}
                onChange={this.handleChangeMaCanBo}
                value={this.state.MaCanBo}
                suffix={<Icon type="search" onClick={this.handleSearchByCode}></Icon>}
              />
            </div>
          </div>
        </div>
        <div className="col-lg-6 col-12 my-1  ">
          <div className="row align-items-center">
            <div className="col-6 col-lg-4 ">Họ và tên</div>
            <div className="col-6 col-lg-8 ">
              <Input disabled value={CanBoData.TenCanBo}></Input>
            </div>
          </div>
        </div>
        <div className="col-lg-6 col-12 my-1  ">
          <div className="row align-items-center">
            <div className="col-6 col-lg-4 ">Học hàm, học vị</div>
            <div className="col-6 col-lg-8 ">
              <Input disabled value={CanBoData.TenHocHamHocVi}></Input>
            </div>
          </div>
        </div>
        <div className="col-lg-6 col-12 my-1  ">
          <div className="row align-items-center">
            <div className="col-6 col-lg-4 ">Chức danh </div>
            <div className="col-6 col-lg-8 ">
              <Input disabled value={CanBoData.TenChucDanh}></Input>
            </div>
          </div>
        </div>
        <div className="col-lg-6 col-12 my-1  ">
          <div className="row align-items-center">
            <div className="col-6 col-lg-4 ">Năm sinh</div>
            <div className="col-6 col-lg-8 ">
              <Input disabled value={CanBoData.NgaySinh ? moment(CanBoData.NgaySinh).format("DD/MM/YYYY") : null}></Input>
            </div>
          </div>
        </div>
        <div className="col-lg-6 col-12 my-1  ">
          <div className="row align-items-center">
            <div className="col-6 col-lg-4 ">Giới tính</div>
            <div className="col-6 col-lg-8 ">
              <Input disabled value={CanBoData.GioiTinhStr}></Input>
            </div>
          </div>
        </div>
        <div className="col-lg-6 col-12 my-1  ">
          <div className="row align-items-center">
            <div className="col-6 col-lg-4 ">Địa chỉ</div>
            <div className="col-6 col-lg-8 ">
              <Input disabled value={CanBoData.DiaChi}></Input>
            </div>
          </div>
        </div>
        <div className="col-lg-6 col-12 my-1  ">
          <div className="row align-items-center">
            <div className="col-6 col-lg-4 ">Email</div>
            <div className="col-6 col-lg-8 ">
              <Input disabled value={CanBoData.Email}></Input>
            </div>
          </div>
        </div>
        <div className="col-lg-6 col-12 my-1  ">
          <div className="row align-items-center">
            <div className="col-6 col-lg-4 ">Điện thoại di động</div>
            <div className="col-6 col-lg-8 ">
              <Input disabled value={CanBoData.DienThoai}></Input>
            </div>
          </div>
        </div>
        <div className="col-lg-6 col-12 my-1  ">
          <div className="row align-items-center">
            <div className="col-6 col-lg-4 ">Fax</div>
            <div className="col-6 col-lg-8 ">
              <Input disabled value={CanBoData.Fax}></Input>
            </div>
          </div>
        </div>
      </div>
    );
  };

  render() {
    const { data } = this.props;
    const { confirmLoading, dataDeXuat, CanBoData } = this.state;
    return (
      <Modal
        maskClosable={false}
        bodyStyle={{ padding: 16 }}
        confirmLoading={confirmLoading}
        width={960}
        title={"Thêm thông tin nhiệm vụ nghiên cứu"}
        visible={true}
        onOk={this.handleOk}
        onCancel={this.handleCancel}
        okText="Lưu"
        cancelText="Huỷ"
      >
        <ValidatorForm
          ref="form"
          onSubmit={this.handleSubmit}
          onError={() => {
            const firstError = document.getElementsByClassName("invalid-error")[0];
            if (firstError) {
              firstError.scrollIntoView({ behavior: "smooth", block: "end", inline: "nearest" });
            }
          }}
        >
          <div className="row ">
            <div className="col-12">
              {/* <Divider orientation="left"> */}
              <Icon type="menu"></Icon> <span className="font-weight-bold ml-2">THÔNG TIN CHUNG</span>
              {/* </Divider> */}
            </div>
            <div className="col-lg-12 col-12 my-1  ">
              <div className="row align-items-center">
                <div className="col-6 col-lg-2 ">
                  Tên nhiệm vụ <span className="text-danger">*</span>
                </div>
                <div className="col-6 col-lg-10 ">
                  <GoInput onChange={this.handleInputChange("TenDeTai")} value={dataDeXuat.TenDeTai} validators={["required"]} errorMessages={["Nội dung bắt buộc"]} />
                </div>
              </div>
            </div>
            <div className="col-lg-6 col-12 my-1  ">
              <div className="row align-items-center">
                <div className="col-6 col-lg-4 ">Mã nhiệm vụ</div>
                <div className="col-6 col-lg-8 ">
                  <GoInput onChange={this.handleInputChange("MaDeTai")} value={dataDeXuat.MaDeTai} />
                </div>
              </div>
            </div>
            <div className="col-lg-6 col-12 my-1  ">
              <div className="row align-items-center">
                <div className="col-6 col-lg-4 ">Loại hình nghiên cứu</div>
                <div className="col-6 col-lg-8 ">
                  <SelectWithApi
                    apiConfig={{
                      api: api.danhSachLoaiHinhNghienCuu,
                      valueField: "Id",
                      nameField: "Name",
                    }}
                    value={dataDeXuat.LoaiHinhNghienCuu}
                    onChange={this.handleInputChange("LoaiHinhNghienCuu")}
                    // validators={["required"]}
                    // errorMessages={["Nội dung bắt buộc"]}
                  ></SelectWithApi>
                </div>
              </div>
            </div>
            <div className="col-lg-6 col-12 my-1  ">
              <div className="row align-items-center">
                <div className="col-6 col-lg-4 ">Lĩnh vực nghiên cứu KHCN</div>
                <div className="col-6 col-lg-8 ">
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
                    onChange={this.handleSelectChange("LinhVucNghienCuu")}
                    value={dataDeXuat.LinhVucNghienCuu}
                    // validators={["required"]} errorMessages={["Nội dung bắt buộc"]}
                  />
                </div>
              </div>
            </div>
            <div className="col-lg-6 col-12 my-1  ">
              <div className="row align-items-center">
                <div className="col-6 col-lg-4 ">Lĩnh vực kinh tế - xã hội</div>
                <div className="col-6 col-lg-8 ">
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
                    onChange={this.handleSelectChange("LinhVucKinhTeXaHoi")}
                    value={dataDeXuat.LinhVucKinhTeXaHoi}
                  />
                </div>
              </div>
            </div>
            <div className=" col-12 my-1  ">
              <div className=" row align-items-center">
                <div className=" col-lg-2">
                  Nhiệm vụ khoa học <span className="text-danger">*</span>
                </div>
                <div className="col-lg-10 ">
                  <TreeSelectWithApi
                    apiConfig={{
                      api: api.danhSachCapDeTai,
                      valueField: "ID",
                      nameField: "Name",
                      filter: { status: true },
                    }}
                    returnFullItem
                    onChange={this.handleSelectNhiemVu}
                    value={dataDeXuat.NhiemVu}
                    customTitle={(item) => {
                      return (
                        <Tooltip title={item.Name} mouseEnterDelay={0.5}>
                          <span className="d-inline-block text-truncate" style={{ width: "100%", transform: "translateY(4px)", color: item.MappingId === 0 ? "#6c757d" : "black" }}>
                            {item.Name}
                          </span>
                        </Tooltip>
                      );
                    }}
                    validators={["required"]}
                    errorMessages={["Nội dung bắt buộc"]}
                  />
                </div>
              </div>
            </div>
            <div className="col-lg-6 col-12 my-1  ">
              <div className="row align-items-center">
                <div className="col-6 col-lg-4 ">Cơ quan chủ quản</div>
                <div className="col-6 col-lg-8 ">
                  <GoInput onChange={this.handleInputChange("CoQuanChuQuan")} value={dataDeXuat.CoQuanChuQuan} />
                </div>
              </div>
            </div>
            <div className="col-lg-6 col-12 my-1  ">
              <div className="row align-items-center">
                <div className="col-6 col-lg-4 ">Đơn vị quản lý khoa học</div>
                <div className="col-6 col-lg-8">
                  <GoInput onChange={this.handleInputChange("DonViQuanLyKhoaHoc")} value={dataDeXuat.DonViQuanLyKhoaHoc} />
                </div>
              </div>
            </div>
          </div>
          <Spin wrapperClassName="col-12 px-0" spinning={this.state.searching}>
            <div className="row">
              <div className="col-12">
                <Icon type="menu"></Icon> <span className="font-weight-bold ml-2">THÔNG TIN CHỦ NHIỆM ĐỀ TÀI</span>
              </div>
              <div className="col-lg-12 col-12 my-1  ">
                <div className="row align-items-center">
                  <div className="col-6 col-lg-2 ">Cơ quan:</div>
                  <div className="col-6 col-lg-10 ">
                    <Radio.Group
                      onChange={(event) => {
                        const { dataDeXuat } = this.state;
                        dataDeXuat.ChuNhiemDeTaiID = null;
                        dataDeXuat.CoQuanChuNhiemID = null;
                        this.setState({ loaichunhiem: event.target.value, CanBoData: {}, dataDeXuat, MaCanBo: null });
                      }}
                      value={this.state.loaichunhiem}
                    >
                      <Radio value="trongtruong">Trong trường</Radio>
                      <Radio value="ngoaitruong">Ngoài trường</Radio>
                    </Radio.Group>
                  </div>
                </div>
              </div>
            </div>

            {this.state.loaichunhiem === "trongtruong" ? this.renderChonChuNhiemTrongTruong() : this.renderChonChuNhiemNgoaiTruong()}
          </Spin>
          <div className="row">
            <div className="col-12">
              <Icon type="menu"></Icon> <span className="font-weight-bold ml-2">THÔNG TIN THÀNH VIÊN THAM GIA</span>
              <div className="float-right">
                <Button onClick={this.handleMinusMember} className="mr-2" icon="minus" size="small" />
                <Button onClick={this.handleAddMember} type="primary" icon="plus" size="small" />
              </div>
              <div className="clearfix"></div>
            </div>
            {/* <div className="col-12 text-danger">{dataDeXuat.VaiTroChuNhiemID || dataDeXuat.ThanhVienNghienCuu.length === 0 ? "" : "Nhiệm vụ khoa học chưa phù hợp, không thể lấy danh sách vai trò, vui lòng chọn lại"}</div> */}
            <div className="col-12">
              {dataDeXuat.ThanhVienNghienCuu.map((item, index) => (
                <div className="row" key={index}>
                  <div className="col-12 col-lg-6 my-1">
                    <div className="row align-items-center">
                      <div className="col-6 col-lg-4 ">Họ và tên</div>
                      <div className="col-6 col-lg-8">
                        <RCSelectWithApi
                          // open
                          optionHeight={80}
                          optionLabelProp="label"
                          apiConfig={{
                            api: api.DanhSachTaiKhoan,
                            valueField: "CanBoID",
                            nameField: "TenCanBo",
                            filter: {
                              TrangThaiID: 0,
                            },
                          }}
                          onClickCreateBtn={() => {
                            this.rowCanBo = index;
                            this.handleAddPeople();
                          }}
                          noInitData
                          data={this.state.danhSachCanBo}
                          showSearch
                          suffixIcon={<i className="fas fa-search"></i>}
                          customOption={(item) => {
                            return (
                              <div className="row align-items-center">
                                <div className="col-2">{item.AnhHoSo ? <Avatar src={item.AnhHoSo} /> : <Avatar icon={<Icon type="user" />} />}</div>
                                <div className="col-10">
                                  <p className="mb-0 ">
                                    <b>{item.TenCanBo}</b>
                                  </p>
                                  <Tooltip title={item.TenChucVu}>
                                    <p className="mb-0 text-truncate">Chức vụ: {item.TenChucVu}</p>
                                  </Tooltip>
                                  <p className="mb-0 ">Đơn vị công tác: {item.TenDonViCongTac}</p>
                                </div>
                              </div>
                            );
                          }}
                          onChange={(value) => {
                            const parseValue = JSON.parse(value);
                            const { dataDeXuat } = this.state;
                            dataDeXuat.ThanhVienNghienCuu[index].LaCanBoTrongTruong = parseValue.LaCanBoTrongTruong;
                            dataDeXuat.ThanhVienNghienCuu[index].CanBoID = parseValue.CanBoID;
                            this.setState({
                              dataDeXuat,
                            });
                          }}
                          value={
                            !dataDeXuat.ThanhVienNghienCuu[index].CanBoID && !dataDeXuat.ThanhVienNghienCuu[index].LaCanBoTrongTruong
                              ? null
                              : JSON.stringify({
                                  CanBoID: dataDeXuat.ThanhVienNghienCuu[index].CanBoID,
                                  LaCanBoTrongTruong: dataDeXuat.ThanhVienNghienCuu[index].LaCanBoTrongTruong,
                                })
                          }
                        />
                      </div>
                    </div>
                  </div>
                  <div className="col-lg-6 col-12 my-1  ">
                    <div className="row align-items-center">
                      <div className="col-6 col-lg-4 ">Vai trò</div>
                      <div className="col-6 col-lg-8">
                        <GoSelect
                          notFoundContent={!dataDeXuat.VaiTroChuNhiemID ? "Không có vai trò phù hợp với NVKH" : "Không có dữ liệu"}
                          data={this.state.danhSachVaiTro}
                          showSearch
                          onChange={(value) => {
                            const { dataDeXuat } = this.state;
                            dataDeXuat.ThanhVienNghienCuu[index].VaiTro = value;
                            this.setState({ dataDeXuat });
                          }}
                          value={dataDeXuat.ThanhVienNghienCuu[index].VaiTro}
                        />
                      </div>
                    </div>
                  </div>
                </div>
              ))}
            </div>
            <div className="col-lg-12 col-12 my-1  ">
              <div className="row align-items-center">
                <div className="col-6 col-lg-2 ">Đơn vị phối hợp</div>
                <div className="col-6 col-lg-10 ">
                  <GoInput onChange={this.handleInputChange("DonViPhoiHop")} value={dataDeXuat.DonViPhoiHop} />
                </div>
              </div>
            </div>
            <div className="col-12">
              <Icon type="menu"></Icon> <span className="font-weight-bold ml-2">THỜI GIAN THỰC HIỆN</span>
            </div>
            <div className="col-lg-6 col-12 my-1  ">
              <div className="row align-items-center">
                <div className="col-6 col-lg-4 ">
                  Năm bắt đầu <span className="text-danger">*</span>
                </div>
                <div className="col-6 col-lg-8 ">
                  <GoDatePicker
                    monthPicker
                    placeholder="Chọn năm"
                    onChange={(date, dateString) => {
                      const { dataDeXuat } = this.state;
                      dataDeXuat.NamBatDau = date ? date.format("MM/YYYY") : null;
                      this.setState({ dataDeXuat });
                    }}
                    value={dataDeXuat.NamBatDau ? moment(dataDeXuat.NamBatDau, "MM/YYYY") : dataDeXuat.NamBatDau}
                    format="MM/YYYY"
                    validators={["required"]}
                    errorMessages={["Nội dung bắt buộc"]}
                  />
                </div>
              </div>
            </div>
            <div className="col-lg-6 col-12 my-1  ">
              <div className="row align-items-center">
                <div className="col-6 col-lg-4 ">
                  Năm kết thúc <span className="text-danger">*</span>
                </div>
                <div className="col-6 col-lg-8">
                  <GoDatePicker
                    monthPicker
                    placeholder="Chọn năm"
                    onChange={(date, dateString) => {
                      const { dataDeXuat } = this.state;
                      dataDeXuat.NamKetThuc = date ? date.format("MM/YYYY") : null;
                      this.setState({ dataDeXuat });
                    }}
                    value={dataDeXuat.NamKetThuc ? moment(dataDeXuat.NamKetThuc, "MM/YYYY") : dataDeXuat.NamKetThuc}
                    format="MM/YYYY"
                    validators={["required"]}
                    errorMessages={["Nội dung bắt buộc"]}
                  />
                </div>
              </div>
            </div>
            <div className="col-12">
              <Icon type="menu"></Icon> <span className="font-weight-bold ml-2">KINH PHÍ</span>
            </div>
            <div className="col-lg-6 col-12 my-1  ">
              <div className="row align-items-center">
                <div className="col-6 col-lg-4 ">Kinh phí DHSP/ NSNN</div>
                <div className="col-6 col-lg-8 ">
                  <GoInput
                    isNumber
                    onChange={this.handleInputChange("KinhPhiDHSP")}
                    value={dataDeXuat.KinhPhiDHSP}
                    validators={["isNumber", "isPositive"]}
                    errorMessages={["Kinh phí phải là dạng số", "Kinh phí phải lớn hơn 0"]}
                  />
                </div>
              </div>
            </div>
            <div className="col-lg-6 col-12 my-1  ">
              <div className="row align-items-center">
                <div className="col-6 col-lg-4 ">Nguồn khác</div>
                <div className="col-6 col-lg-8">
                  <GoInput
                    isNumber
                    onChange={this.handleInputChange("NguonKhac")}
                    value={dataDeXuat.NguonKhac}
                    validators={["isNumber", "isPositive"]}
                    errorMessages={["Nguồn khác phải là dạng số", "Nguồn khác phải lớn hơn 0"]}
                  />
                </div>
              </div>
            </div>
            <div className="col-12">
              <Icon type="menu"></Icon> <span className="font-weight-bold ml-2">MỤC TIÊU</span>
            </div>
            <div className="col-12 my-1">
              <GoEditor value={dataDeXuat.MucTieu} onDone={this.onEditorDone("MucTieu")}></GoEditor>
            </div>
            <div className="col-12">
              <Icon type="menu"></Icon> <span className="font-weight-bold ml-2">CÁC NỘI DUNG CHÍNH</span>
              <div className="float-right">
                <Button
                  onClick={() => {
                    const { dataDeXuat } = this.state;
                    dataDeXuat.CacNoiDungChinh.splice(dataDeXuat.CacNoiDungChinh.length - 1, 1);
                    this.setState({ dataDeXuat });
                  }}
                  className="mr-2"
                  icon="minus"
                  size="small"
                />
                <Button
                  onClick={() => {
                    const { dataDeXuat } = this.state;
                    dataDeXuat.CacNoiDungChinh.push({ TenNoiDung: "", Mota: "" });
                    this.setState({ dataDeXuat });
                  }}
                  type="primary"
                  icon="plus"
                  size="small"
                />
              </div>
            </div>
            <div className="col-12">
              {dataDeXuat.CacNoiDungChinh.map((item, index) => {
                return (
                  <div className="row" key={index}>
                    <div className="col-lg-12 col-12 my-1  ">
                      <div className="row align-items-center">
                        <div className="col-6 col-lg-2 ">Tên nội dung</div>
                        <div className="col-6 col-lg-10 ">
                          <GoInput
                            value={item.TenNoiDung}
                            onChange={(event) => {
                              const { dataDeXuat } = this.state;
                              dataDeXuat.CacNoiDungChinh[index].TenNoiDung = event.target.value;
                              this.setState({ dataDeXuat });
                            }}
                          />
                        </div>
                      </div>
                    </div>

                    <div className="col-lg-12 col-12 my-1  ">
                      <div className="row align-items-center">
                        <div className="col-6 col-lg-2 ">Mô tả</div>
                        <div className="col-6 col-lg-10 ">
                          <GoEditor
                            value={item.MoTa}
                            onDone={(value) => {
                              const { dataDeXuat } = this.state;
                              dataDeXuat.CacNoiDungChinh[index].Mota = value;
                              this.setState({ dataDeXuat });
                            }}
                          />
                        </div>
                      </div>
                    </div>
                  </div>
                );
              })}
            </div>

            <div className="col-12">
              <Icon type="menu"></Icon> <span className="font-weight-bold ml-2">SẢN PHẨM ĐĂNG KÝ</span>
            </div>
            <div className="col-12 my-1">
              <GoEditor value={dataDeXuat.SanPhamDangKy} onDone={this.onEditorDone("SanPhamDangKy")}></GoEditor>
            </div>
            <div className="col-12">
              <Icon type="menu"></Icon> <span className="font-weight-bold ml-2">KHẢ NĂNG ỨNG DỤNG CỦA ĐỀ TÀI</span>
            </div>
            <div className="col-12 my-1">
              <GoEditor value={dataDeXuat.KhaNangUngDung} onDone={this.onEditorDone("KhaNangUngDung")}></GoEditor>
            </div>
            <div className="col-12">
              <Icon type="menu"></Icon> <span className="font-weight-bold ml-2">TÀI LIỆU ĐÍNH KÈM (Quyết định, Hợp đồng, Thuyết minh, ...)</span>
            </div>
            {dataDeXuat.FileDinhKem.map((item, index) => {
              return (
                <div className="col-12 my-1" key={index}>
                  <div className=" row align-items-center">
                    <div className=" col-lg-2">File đính kèm</div>
                    <div className="col-lg-10 ">
                      <Upload
                        multiple
                        beforeUpload={(file) => {
                          return false;
                        }}
                        fileList={dataDeXuat.FileDinhKem[index].files || []}
                        onChange={async ({ file, fileList }) => {
                          const { dataDeXuat } = this.state;
                          if (file.status === "removed") {
                            const fileIndex = dataDeXuat.FileDinhKem[index].files.findIndex((d) => d.uid === file.uid);
                            dataDeXuat.FileDinhKem[index].files.splice(fileIndex, 1);
                            dataDeXuat.FileDinhKem[index].TenFileGoc = fileList.map((item) => item.name).join(";");
                            this.setState({ dataDeXuat });
                          } else {
                            const fileType = await checkFileType(file);
                            if (!fileType.valid) {
                              message.error(`File đính kèm không hợp lệ. (Chỉ được đính kèm file: ${fileType.fileTypes})`);
                              return;
                            }
                            const result = await checkFilesSize(file);
                            if (!result.valid) {
                              message.error(`File đính kèm phải nhỏ hơn ${result.limitFileSize} MB. (${file.name})`);
                              return;
                            }
                            dataDeXuat.FileDinhKem[index].files.push(file);
                            dataDeXuat.FileDinhKem[index].TenFileGoc = fileList.map((item) => item.name).join(";");
                            this.setState({ dataDeXuat });
                          }
                        }}
                      >
                        <Button>
                          <Icon type="upload" /> Chọn file
                        </Button>
                      </Upload>
                    </div>
                  </div>
                  <div className=" row align-items-center my-2">
                    <div className=" col-lg-2">Nội dung</div>
                    <div className="col-lg-10 ">
                      <TextArea
                        value={dataDeXuat.FileDinhKem[index].NoiDung}
                        onChange={(event) => {
                          const { dataDeXuat } = this.state;
                          dataDeXuat.FileDinhKem[index].NoiDung = event.target.value;

                          this.setState({ dataDeXuat });
                        }}
                      ></TextArea>
                    </div>
                  </div>
                </div>
              );
            })}
            <div className="col-12 text-right">
              <div className="btn btn-sm btn-outline-primary" onClick={this.handleAddFileField}>
                Thêm file đính kèm
              </div>
            </div>
          </div>

          <button className="d-none" ref={this.submitBtn} type="submit">
            submit
          </button>
        </ValidatorForm>
      </Modal>
    );
  }
}

export default KQNghienCuuModal;
