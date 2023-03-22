import React from "react";
import { Icon, Button, Input, Divider, Tooltip, message, Avatar, Spin, Table, Radio, Modal, Empty } from "antd";
import { Select } from "antd4";
import { ValidatorForm } from "react-form-validator-core";
import api, { TrangThai } from "./config";
import * as moment from "moment";
import LayoutWrapper from "../../../components/utility/layoutWrapper";
import PageHeader from "../../../components/utility/pageHeader";
import Box from "../../../components/utility/box";
import { Collapse } from "antd";
import { withAPI } from "../../components/withAPI";
import { GoTreeSelect, GoInput, GoSelect, withInputToText, GoEditor, GoDatePicker, RCSelectWithApi, GoSelect4 } from "../../components/index";
import lodash, { isArray } from "lodash";
import { convertFileTable } from "../../../helpers/utility";
import { TienDoThucHien, KinhPhi, SanPhamDeTai, KetQuaChuyenGiao, KetQuaDanhGia, DanhGiaGiaiDoan, KetQuaNghienCuu } from "./ChiTietDeTaiComponent/index";
import { Prompt } from "react-router";
import { ModalAddEdit } from "./ThemCanBoModal";
import { connect } from "react-redux";
import { getRoleByKey2 } from "../../../helpers/utility";
import AddFileModal from "./Themtailieudinhkem";
import apiConfig from "../ThamSoHeThong/config";
const TreeSelectWithApi = withAPI(GoTreeSelect);

const { Panel } = Collapse;
const SelectWithApi = withAPI(GoSelect);

/* eslint-disable react/prefer-stateless-function */
class GoModal extends React.Component {
  isChanged = false;
  isTrangThaiChanged = false;
  constructor(props) {
    super(props);
    document.title = "Chi tiết nhiệm vụ nghiên cứu";
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
        CapQuanLy: null,
        NhiemVu: null,
        CoQuanChuQuan: "",
        DonViQuanLyKhoaHoc: "",
        ChuNhiemDeTaiID: null,
        CoQuanChuNhiemID: null,
        VaiTroChuNhiemID: null,
        NamBatDau: null,
        NamKetThuc: null,
        KinhPhiDHSP: null,
        NguonKhac: null,
        MucTieu: "",
        CacNoiDungChinh: [{ Mota: "", TenNoiDung: "" }],
        SanPhamDangKy: "",
        KhaNangUngDung: "",
        ThanhVienNghienCuu: [
          // {
          //   CanBoID: "123",
          //   VaiTro: "1",
          //   LaCanBoTrongTruong: "0",
          // },
        ],
        DonViPhoiHop: null,
        FileDinhKem: [{ TenFileGoc: "", NoiDung: "", files: [] }],
      },
      searching: false,
      danhSachVaiTro: [],
      danhSachCanBo: [],
      loadedDanhSachCanBo: false,
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

    this.getDetail();
    apiConfig.GetByKey({ ConfigKey: "ID_NHOM_QUYEN_QLKH" }).then((res) => {
      if (res.data.Data.ConfigValue === localStorage.getItem("role_id")) {
        this.setState({ isQuanLy: true });
      }
    });
  }
  componentWillUnmount() {}

  getDetail = () => {
    const { id } = this.props.match.params;
    this.Id = id;
    api.chiTietDeTai({ DeTaiID: id }).then((res) => {
      if (res.data && res.data.Status === 1) {
        const { CanBoData } = res.data.Data;
        const dataDeXuat = res.data.Data;
        this.setState({
          dataDeXuat,
          loading: false,
          CanBoData: CanBoData || {},
          MaCanBo: CanBoData ? CanBoData.MaCanBo : null,
          loaichunhiem: dataDeXuat.CoQuanChuNhiemID === 999999999 ? "ngoaitruong" : "trongtruong",
        });
        this.originalData = lodash.cloneDeep(res.data.Data);
        this.getVaiTro({ id: this.originalData.NhiemVu });
      } else {
        this.setState({ loading: false });
        message.error(`${res.data && res.data.Message ? res.data.Message : "Lấy chi tiết đề tài thất bại"}`);
      }
    });
  };
  getVaiTro = (param) => {
    api.danhMucCapDeTaiByID(param).then((res) => {
      if (res.data && res.data.Status === 1) {
        const { MappingId } = res.data.Data;
        api.danhSachVaiTroTheoNhiemVu({ CategoryId: MappingId }).then((res) => {
          if (res) {
            // format data core trả về phù hợp với select
            const danhSachVaiTro = res.data.Data.filter((d) => d.Id !== this.originalData.VaiTroChuNhiemID).map((item) => ({ ...item, ...{ value: item.Id, text: item.Name } }));
            if (res.data.Status === 1) {
              this.setState({ danhSachVaiTro });
            } else {
              message.error(`${res.message || "Lấy danh sách vai trò nhiệm vụ thất bại"}`);
            }
          } else {
            message.error("Lấy danh sách vai trò nhiệm vụ thất bại");
          }
        });
      } else {
        message.error(`${res.data && res.data.Message ? res.data.Message : "Lấy danh mục vai trò thất bại"}`);
      }
    });
  };
  onDeleteFile = (file) => {
    Modal.confirm({
      content: "Bạn có chắc chắn muốn xoá file đính kèm này?",
      okText: "Xoá",
      cancelText: "Huỷ",
      onOk: () => {
        const { FileDinhKemID } = file;
        api.xoaFileDinhKem({ FileDinhKemID: FileDinhKemID }).then((res) => {
          if (!res || !res.data || res.data.Status !== 1) {
            this.setState({ confirmLoading: false });
            message.error(`${res && res.data ? res.data.Message : "Lỗi hệ thống"}`);
          } else {
            message.success("Xoá thành công");
            this.setState({ confirmLoading: false });
            this.getDetail();
            // this.props.onClose();
          }
        });
      },
    });
  };
  handleCancel = () => {
    this.props.onClose();
  };
  handleOk = () => {
    this.submitBtn.current.click();
  };
  handleCloseModal = (value = null, CanBoID = null) => {
    let { dataDeXuat, danhSachCanBo } = this.state;
    if (value === "ThemNKH") {
      api.danhSachAllCanBo({ PageSize: 999999 }).then((res) => {
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
    this.getDetail();
    this.modal.destroy();
  };
  handleSubmit = () => {
    if (this.state.CanBoData == {}) {
      message.warning("Vui lòng chọn chủ nhiệm đề tài hợp lệ");
      return;
    }
    const {
      DeTaiID,
      MaDeTai,
      TenDeTai,
      LoaiHinhNghienCuu,
      LinhVucNghienCuu,
      LinhVucKinhTeXaHoi,
      CapQuanLy,
      NhiemVu,
      VaiTroChuNhiemID,
      CoQuanChuQuan,
      DonViQuanLyKhoaHoc,
      ChuNhiemDeTaiID,
      NamBatDau,
      NamKetThuc,
      KinhPhiDHSP,
      NguonKhac,
      MucTieu,
      CacNoiDungChinh,
      SanPhamDangKy,
      KhaNangUngDung,
      ThanhVienNghienCuu,
      TrangThai,
      CoQuanChuNhiemID,
      DonViPhoiHop,
    } = this.state.dataDeXuat;
    const newData = {
      DeTaiID,
      MaDeTai,
      TenDeTai,
      LoaiHinhNghienCuu,
      LinhVucNghienCuu,
      LinhVucKinhTeXaHoi,
      CapQuanLy,
      NhiemVu,
      VaiTroChuNhiemID,
      CoQuanChuQuan,
      DonViQuanLyKhoaHoc,
      ChuNhiemDeTaiID,
      NamBatDau,
      NamKetThuc,
      KinhPhiDHSP,
      NguonKhac,
      MucTieu,
      CacNoiDungChinh,
      SanPhamDangKy,
      KhaNangUngDung,
      ThanhVienNghienCuu,
      TrangThai,
      CoQuanChuNhiemID,
      DonViPhoiHop,
    };
    this.setState({ confirmLoading: true });
    api.capNhatDeTai(newData).then((res) => {
      if (!res || !res.data || res.data.Status !== 1) {
        this.setState({ confirmLoading: false });
        message.error(`${res && res.data ? res.data.Message : "Lỗi hệ thống"}`);
      } else {
        message.success("Cập nhật thành công");

        this.isChanged = false;
        this.isTrangThaiChanged = false;
        if (this.path) {
          this.props.history.push(this.path);
        } else {
          this.getDetail();
          this.setState({ confirmLoading: false });
        }
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
          message.error(`${res.data ? "Không tìm thấy cán bộ" : "Lỗi hệ thống"}`);
          this.setState({ searching: false });
        }
      } else {
        message.error(`Không thể tìm kiếm cán bộ`);
        this.setState({ searching: false });
      }
    });
  };
  handleOpenModal = (name) => {
    switch (name) {
      case "AddFileModal":
        this.modal = Modal.confirm({
          icon: <i />,
          content: <AddFileModal NghiepVuID={this.Id} rowCanBo={this.rowCanBo} onClose={this.handleCloseModal} />,
        });
        break;

      default:
        break;
    }
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
      // console.log(dataDeXuat.VaiTroChuNhiemID);
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
      // this.setState({
      //   dataDeXuat,
      // });
    }
  };
  handleSelectChange = (name) => (value) => {
    const { dataDeXuat } = this.state;
    dataDeXuat[name] = value;
    this.setState({
      dataDeXuat,
    });
  };
  handleAddPeople = (laChuNhiem = false) => {
    this.modal = Modal.confirm({
      icon: <i />,
      content: <ModalAddEdit onClose={this.handleCloseModal} laChuNhiem={laChuNhiem}></ModalAddEdit>,
    });
  };
  handleSelectMember = (index) => (value) => {
    const { dataDeXuat } = this.state;
    dataDeXuat.ThanhVienNghienCuu[index].CanBoID = value;
    this.setState({
      dataDeXuat,
    });
  };
  handleAddMember = () => {
    const { dataDeXuat } = this.state;
    dataDeXuat.ThanhVienNghienCuu.push({ CanBoID: null, VaiTro: null, LaCanBoTrongTruong: null });
    this.setState({ dataDeXuat });
  };
  handleMinusMember = () => {
    const { dataDeXuat } = this.state;
    dataDeXuat.ThanhVienNghienCuu.splice(dataDeXuat.ThanhVienNghienCuu.length - 1, 1);
    this.setState({ dataDeXuat });
  };
  onEditorDone = (name) => (value) => {
    const { dataDeXuat } = this.state;
    dataDeXuat[name] = value;
    this.setState({ dataDeXuat });
  };
  handleUpdateTrangThai = () => {
    const { TrangThai } = this.state.dataDeXuat;
    api.capNhatTrangThaiDeTai({ DeTaiID: this.Id, TrangThai }).then((res) => {
      if (!res || !res.data || res.data.Status !== 1) {
        message.error(`${res && res.data ? res.data.Message : "Cập nhật thất bại"}`);
      } else {
        message.success("Cập nhật thành công");
        this.getDetail();
      }
    });
  };
  renderFileTable = (data) => {
    const columns = [
      {
        title: "STT",
        dataIndex: "index",
        key: "index",
        render: (text, record, index) => <p>{index + 1}</p>,
      },
      {
        title: "Cán bộ thực hiện",
        dataIndex: "TenNguoiTao",
        key: "TenNguoiTao",
      },
      {
        title: "Ngày thực hiện",
        dataIndex: "NgayTao",
        key: "NgayTao",
        render: (text, record, index) => <p>{moment(text).format("DD/MM/YYYY")}</p>,
      },
      {
        title: "Nội dung",
        dataIndex: "NoiDung",
        key: "NoiDung",
      },
      {
        title: "File đính kèm",
        dataIndex: "files",
        key: "files",
        render: (text, record, index) => (
          <div>
            {record.files.map((item, index) => (
              <span className="border border-primary rounded mx-1 my-1 d-inline-block p-1" key={index}>
                <a download={item.TenFileGoc} target="_blank" href={item.FileUrl}>
                  {item.TenFileGoc}
                </a>
                <Icon onClick={() => this.onDeleteFile(item)} className="pointer ml-1" type="close"></Icon>
              </span>
            ))}
          </div>
        ),
      },
    ];
    return <Table locale={{ emptyText: "Không có dữ liệu" }} pagination={false} rowKey="FileDinhKemID" bordered dataSource={data} columns={columns}></Table>;
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
                disabled={!role.edit}
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
    const { data, role } = this.props;
    const { confirmLoading, dataDeXuat, CanBoData } = this.state;
    const { FileDinhKem } = dataDeXuat;

    const detailOrigin = lodash.cloneDeep(this.originalData);
    const detailData = lodash.cloneDeep(this.state.dataDeXuat);
    if (detailData) {
      delete detailData.TrangThai;
    }
    if (detailOrigin) {
      delete detailOrigin.TrangThai;
    }

    this.isChanged = JSON.stringify(detailData) !== JSON.stringify(detailOrigin);
    this.isTrangThaiChanged = this.originalData && JSON.stringify(this.originalData.TrangThai) !== JSON.stringify(this.state.dataDeXuat.TrangThai);

    const groupedFileDinhKem = convertFileTable(FileDinhKem, ["FileDinhKemID", "TenFileGoc", "FileUrl", "NgayTao", "TenNguoiTao", "NguoiTaoID", "NoiDung"]);
    return (
      <LayoutWrapper>
        <Prompt
          when={this.isChanged || this.isTrangThaiChanged}
          message={(location) => {
            if (location.pathname === "/") {
              return true;
            }
            if (this.isChanged || this.isTrangThaiChanged) {
              this.path = location.pathname;
              Modal.confirm({
                content: "Bạn có muốn lưu dữ liệu thay đổi trước khi thoát ?",
                onOk: () => {
                  this.submitBtn.current.click();
                },
                onCancel: () => {
                  this.isChanged = false;
                  this.isTrangThaiChanged = false;
                  this.props.history.push(location.pathname);
                },
                okText: "Có",
                cancelText: "Không",
              });
              return false;
            }
            return true;
          }}
        />
        <PageHeader>Quản lý nhiệm vụ nghiên cứu</PageHeader>
        <Box>
          <Spin spinning={this.state.loading}>
            <div id="antd-v4" className="custom-collapse ">
              <Collapse defaultActiveKey={["0", "1", "2", "3", "4", "5", "6", "7", "8"]} expandIconPosition={"right"}>
                <Panel header="Thông tin đề tài" key="1">
                  <ValidatorForm
                    className="antd-v4"
                    instantValidate={false}
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
                            <GoInput
                              disabled={!role.edit}
                              withTextToInput
                              onChange={this.handleInputChange("TenDeTai")}
                              value={dataDeXuat.TenDeTai}
                              validators={["required"]}
                              errorMessages={["Nội dung bắt buộc"]}
                            />
                          </div>
                        </div>
                      </div>
                      <div className="col-lg-6 col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-4 ">Mã nhiệm vụ</div>
                          <div className="col-6 col-lg-8 ">
                            <GoInput disabled={!role.edit} withTextToInput onChange={this.handleInputChange("MaDeTai")} value={dataDeXuat.MaDeTai} />
                          </div>
                        </div>
                      </div>
                      <div className="col-lg-6 col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-4 ">Loại hình nghiên cứu</div>
                          <div className="col-6 col-lg-8 ">
                            <SelectWithApi
                              withTextToInput
                              apiConfig={{
                                api: api.danhSachLoaiHinhNghienCuu,
                                valueField: "Id",
                                nameField: "Name",
                              }}
                              value={dataDeXuat.LoaiHinhNghienCuu === 0 ? null : dataDeXuat.LoaiHinhNghienCuu}
                              onChange={this.handleInputChange("LoaiHinhNghienCuu")}
                              disabled={!role.edit}
                            ></SelectWithApi>
                          </div>
                        </div>
                      </div>
                      <div className="col-lg-6 col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-4 ">Lĩnh vực nghiên cứu KHCN</div>
                          <div className="col-6 col-lg-8 ">
                            <TreeSelectWithApi
                              disabled={!role.edit}
                              withTextToInput
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
                              value={dataDeXuat.LinhVucNghienCuu === 0 ? null : dataDeXuat.LinhVucNghienCuu}
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
                              disabled={!role.edit}
                              withTextToInput
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
                              value={dataDeXuat.LinhVucKinhTeXaHoi === 0 ? null : dataDeXuat.LinhVucKinhTeXaHoi}
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
                              disabled={!role.edit}
                              withTextToInput
                              apiConfig={{
                                api: api.danhSachCapDeTai,
                                valueField: "ID",
                                nameField: "Name",
                                filter: {
                                  status: true,
                                },
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
                            <GoInput disabled={!role.edit} withTextToInput onChange={this.handleInputChange("CoQuanChuQuan")} value={dataDeXuat.CoQuanChuQuan} />
                          </div>
                        </div>
                      </div>
                      <div className="col-lg-6 col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-4 ">Đơn vị quản lý khoa học</div>
                          <div className="col-6 col-lg-8">
                            <GoInput disabled={!role.edit} withTextToInput onChange={this.handleInputChange("DonViQuanLyKhoaHoc")} value={dataDeXuat.DonViQuanLyKhoaHoc} />
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
                      {/* {this.renderChonChuNhiemNgoaiTruong()} */}
                    </Spin>
                    <div className="row">
                      <div className="col-12">
                        <Icon type="menu"></Icon> <span className="font-weight-bold ml-2">THÔNG TIN THÀNH VIÊN THAM GIA</span>
                        <div className="float-right">
                          <Button disabled={!role.edit} onClick={this.handleMinusMember} className="mr-2" icon="minus" size="small" />
                          <Button disabled={!role.edit} onClick={this.handleAddMember} type="primary" icon="plus" size="small" />
                        </div>
                        <div className="clearfix"></div>
                      </div>
                      {/* <div className="col-12 text-danger">{dataDeXuat.VaiTroChuNhiemID || dataDeXuat.ThanhVienNghienCuu.length === 0 ? "" : "Nhiệm vụ khoa học chưa phù hợp, không thể lấy danh sách vai trò, vui lòng chọn lại"}</div> */}
                      <div className="col-12">
                        {this.state.loadedDanhSachCanBo
                          ? dataDeXuat.ThanhVienNghienCuu.map((item, index) => (
                              <div className="row" key={index}>
                                <div className="col-12 col-lg-6 my-1">
                                  <div className="row align-items-center">
                                    <div className="col-6 col-lg-4 ">Họ và tên</div>
                                    <div className="col-6 col-lg-8">
                                      <RCSelectWithApi
                                        disabled={!role.edit}
                                        optionHeight={80}
                                        withTextToInput
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
                                                  <p className="mb-0  text-truncate">Chức vụ: {item.TenChucVu}</p>
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
                                        disabled={!role.edit}
                                        withTextToInput
                                        data={this.state.danhSachVaiTro}
                                        showSearch
                                        onChange={(value) => {
                                          const { dataDeXuat } = this.state;
                                          dataDeXuat.ThanhVienNghienCuu[index].VaiTro = value;
                                          this.setState({ dataDeXuat });
                                        }}
                                        value={dataDeXuat.ThanhVienNghienCuu[index].VaiTro === 0 ? null : dataDeXuat.ThanhVienNghienCuu[index].VaiTro}
                                      />
                                    </div>
                                  </div>
                                </div>
                                <div className="col-lg-6 col-12 my-1  ">
                                  <div className="row align-items-center">
                                    <div className="col-6 col-lg-4 ">Đơn vị công tác</div>
                                    <div className="col-6 col-lg-8">
                                      {this.state.danhSachCanBo.find((d) => d.CanBoID === dataDeXuat.ThanhVienNghienCuu[index].CanBoID)
                                        ? this.state.danhSachCanBo.find((d) => d.CanBoID === dataDeXuat.ThanhVienNghienCuu[index].CanBoID).TenDonViCongTac
                                        : ""}
                                    </div>
                                  </div>
                                </div>
                              </div>
                            ))
                          : null}
                      </div>
                      <div className="col-lg-12 col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-2 ">Đơn vị phối hợp</div>
                          <div className="col-6 col-lg-10 ">
                            <GoInput disabled={!role.edit} withTextToInput onChange={this.handleInputChange("DonViPhoiHop")} value={dataDeXuat.DonViPhoiHop} />
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
                              disabled={!role.edit}
                              withTextToInput
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
                            <Tooltip
                              placement="topLeft"
                              title={`${dataDeXuat.SoLanGiaHanThucHien === 2 ? "Đã hết số lần gia hạn thực hiện" : `Số lần ra hạn còn lại: ${2 - dataDeXuat.SoLanGiaHanThucHien}`} `}
                            >
                              <GoDatePicker
                                monthPicker
                                placeholder="Chọn năm"
                                disabled={!role.edit || dataDeXuat.SoLanGiaHanThucHien === 2 || !this.state.isQuanLy}
                                withTextToInput
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
                            </Tooltip>
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
                              disabled={!role.edit}
                              withTextToInput
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
                              disabled={!role.edit}
                              withTextToInput
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
                        <GoEditor disabled={!role.edit} value={dataDeXuat.MucTieu} onDone={this.onEditorDone("MucTieu")}></GoEditor>
                      </div>
                      <div className="col-12">
                        <Icon type="menu"></Icon> <span className="font-weight-bold ml-2">CÁC NỘI DUNG CHÍNH</span>
                        <div className="float-right">
                          <Button
                            disabled={!role.edit}
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
                            disabled={!role.edit}
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
                                      disabled={!role.edit}
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
                                      disabled={!role.edit}
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

                      {/* <div className="col-12 my-1">
                        <GoEditor disabled={!role.edit} value={dataDeXuat.CacNoiDungChinh} onDone={this.onEditorDone("CacNoiDungChinh")}></GoEditor>
                      </div> */}
                      <div className="col-12">
                        <Icon type="menu"></Icon> <span className="font-weight-bold ml-2">SẢN PHẨM ĐĂNG KÝ</span>
                      </div>
                      <div className="col-12 my-1">
                        <GoEditor disabled={!role.edit} value={dataDeXuat.SanPhamDangKy} onDone={this.onEditorDone("SanPhamDangKy")}></GoEditor>
                      </div>
                      <div className="col-12">
                        <Icon type="menu"></Icon> <span className="font-weight-bold ml-2">KHẢ NĂNG ỨNG DỤNG CỦA ĐỀ TÀI</span>
                      </div>
                      <div className="col-12 my-1">
                        <GoEditor disabled={!role.edit} value={dataDeXuat.KhaNangUngDung} onDone={this.onEditorDone("KhaNangUngDung")}></GoEditor>
                      </div>
                      <div className="col-12 my-1">
                        <Icon type="menu"></Icon> <span className="font-weight-bold ml-2">TÀI LIỆU ĐÍNH KÈM (Quyết định, Hợp đồng, Thuyết minh, ...)</span>
                        <div className="float-right">
                          <Button
                            disabled={!role.edit}
                            onClick={() => {
                              this.handleOpenModal("AddFileModal");
                            }}
                            className=""
                          >
                            Thêm file đính kèm
                          </Button>
                        </div>
                        <div className="clearfix"></div>
                      </div>
                      <div className="col-12">{this.renderFileTable(groupedFileDinhKem)}</div>
                      <div className=" col-12 text-right ">
                        <button className="d-none" ref={this.submitBtn} type="submit">
                          submit
                        </button>
                        <div className="m-2" style={{ display: this.isChanged ? "block" : "none" }}>
                          <Button
                            loading={this.state.confirmLoading}
                            type="primary"
                            onClick={() => {
                              this.submitBtn.current.click();
                            }}
                          >
                            Lưu
                          </Button>
                        </div>
                      </div>
                    </div>
                  </ValidatorForm>
                </Panel>
                <Panel header="Tiến độ thực hiện" key="2">
                  <TienDoThucHien
                    role={role}
                    DeTaiID={this.Id}
                    data={dataDeXuat.TienDoThucHien}
                    refresh={() => {
                      this.getDetail();
                    }}
                  ></TienDoThucHien>
                </Panel>
                <Panel header="Kinh phí" key="3">
                  <KinhPhi
                    role={role}
                    DeTaiID={this.Id}
                    data={dataDeXuat.KinhPhi}
                    refresh={() => {
                      this.getDetail();
                    }}
                  ></KinhPhi>
                </Panel>
                {/* <Panel header="Sản phẩm đề tài" key="4">
                  <SanPhamDeTai
                    role={role}
                    DeTaiID={this.Id}
                    data={dataDeXuat.SanPhamDeTai}
                    refresh={() => {
                      this.getDetail();
                    }}
                  ></SanPhamDeTai>
                </Panel> */}
                <Panel header="Kết quả nghiên cứu" key="6">
                  {this.state.loading ? (
                    ""
                  ) : (
                    <KetQuaNghienCuu
                      role={role}
                      DeTaiID={this.Id}
                      data={dataDeXuat}
                      refresh={() => {
                        this.getDetail();
                      }}
                    ></KetQuaNghienCuu>
                  )}
                </Panel>
                <Panel header="Kết quả chuyển giao đề tài" key="5">
                  <KetQuaChuyenGiao
                    role={role}
                    DeTaiID={this.Id}
                    data={dataDeXuat.KetQuaChuyenGiao}
                    refresh={() => {
                      this.getDetail();
                    }}
                  ></KetQuaChuyenGiao>
                </Panel>

                <Panel header="Đánh giá giai đoạn, tự đánh giá" key="7">
                  <DanhGiaGiaiDoan
                    role={role}
                    DeTaiID={this.Id}
                    data={dataDeXuat.DanhGiaGiaiDoan}
                    refresh={() => {
                      this.getDetail();
                    }}
                  ></DanhGiaGiaiDoan>
                </Panel>
                <Panel header="Đánh giá, nghiệm thu" key="8">
                  {this.state.loading ? (
                    ""
                  ) : (
                    <KetQuaDanhGia
                      role={role}
                      DeTaiID={this.Id}
                      data={dataDeXuat.KetQuaDanhGia}
                      refresh={() => {
                        this.getDetail();
                      }}
                    ></KetQuaDanhGia>
                  )}
                </Panel>
              </Collapse>
            </div>
          </Spin>
          <div className="text-center my-1">
            <Button
              onClick={() => {
                this.props.history.goBack();
              }}
            >
              Quay lại
            </Button>
          </div>
        </Box>
      </LayoutWrapper>
    );
  }
}
function mapStateToProps(state) {
  const role = getRoleByKey2("ql-de-tai");
  state.QLDeTai.role = role;

  return {
    ...state.QLDeTai,
  };
}
export default connect(mapStateToProps, {})(GoModal);
