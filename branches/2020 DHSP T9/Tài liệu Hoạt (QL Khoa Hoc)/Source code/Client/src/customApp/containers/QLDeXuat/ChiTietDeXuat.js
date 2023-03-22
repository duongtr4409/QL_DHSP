import React from "react";
import { Modal, Icon, Button, message, Spin, Table, Collapse } from "antd";
import { connect } from "react-redux";
import { convertFileTable, getRoleByKey2 } from "../../../helpers/utility";
import * as actions from "../../redux/QLDeXuat/actions";
import GoInput from "../../components/GoInput/index";
import GoSelect from "../../components/GoSelect/index";
import GoEditor from "../../components/GoEditor/editor";
import { GoDatePicker } from "../../components/index";
import { ValidatorForm } from "react-form-validator-core";
import api, { TrangThai } from "./config";
import apiConfig from "../ThamSoHeThong/config";
import * as moment from "moment";
import LayoutWrapper from "../../../components/utility/layoutWrapper";
import PageHeader from "../../../components/utility/pageHeader";
import Box from "../../../components/utility/box";

import { withAPI } from "../../components/withAPI";
import { GoTreeSelect } from "../../components/index";

import lodash from "lodash";
import AddFileModal from "./Themtailieudinhkem";
import { Prompt } from "react-router";
import LichSuChinhSua from "./LichSuChinhSua";
const TreeSelectWithApi = withAPI(GoTreeSelect);
const { Panel } = Collapse;
const SelectWithApi = withAPI(GoSelect);

class ChiTietDeXuat extends React.Component {
  modal = null;
  Id;
  originalData = null;
  isChanged = false;
  constructor(props) {
    document.title = "Chi tiết đề xuất đề tài";
    super(props);
    this.submitBtn = React.createRef();
    this.isBackButtonClicked = false;
    this.state = {
      confirmLoading: false,
      dataDeXuat: {
        MaDeXuat: "",
        CoQuanID: "",
        TenDeXuat: "",
        LinhVucKinhTeXaHoi: null,
        LinhVucNghienCuu: null,
        CapQuanLy: null,
        NguoiDeXuat: null,
        KinhPhiDuKien: null,
        ThoiGianNghienCuu: null,
        TinhCapThiet: "",
        MucTieu: "",
        SanPham: "",
        FileDinhKem: [],
        NgayDeXuat: null,
        NoiDung: null,
        DiaChiUngDung: "",
        ThoiGianThucHienTu: "",
        ThoiGianThucHienDen: "",
        ThuocChuongTrinh: "",
      },
      loading: true,
      DSLoaiKQ: [
        {
          text: "Bài báo",
          value: "Baibao",
        },
      ],
    };
  }
  componentDidMount() {
    this.onInitData();

    // window.history.pushState(null, null, window.location.pathname);
    // window.addEventListener("popstate", this.onBackButtonEvent);
  }
  componentWillUnmount() {
    // window.removeEventListener("popstate", this.onBackButtonEvent);
  }
  onBackButtonEvent = (e) => {
    e.preventDefault();
    const needSave = this.isChanged || this.isTrangThaiChanged;
    if (!this.isBackButtonClicked && needSave) {
      Modal.confirm({
        content: "Bạn có muốn lưu dữ liệu thay đổi trước khi thoát?",
        okText: "Lưu",
        cancelText: "Hủy",
        onOk: () => {
          this.isBackButtonClicked = true;
          this.handleSubmit(true);
        },
        onCancel: () => {
          this.props.history.push("/dashboard/ql-de-tai");
        },
      });
    } else {
      this.props.history.push("/dashboard/ql-de-tai");
    }
  };

  onInitData = () => {
    const { id } = this.props.match.params;
    this.Id = id;
    api.chiTietDeXuat({ DeXuatID: id }).then((res) => {
      if (res.data && res.data.Status === 1) {
        this.originalData = lodash.cloneDeep(res.data.Data);
        // this.setState({ dataDeXuat: res.data.Data, loading: false });
        if (res.data.Data.TrangThai === 6) {
          apiConfig.GetByKey({ ConfigKey: "ID_NHOM_QUYEN_QLKH" }).then((res) => {
            if (res.data.Data.ConfigValue === localStorage.getItem("role_id")) {
              this.setState({ canEdit: true });
            }
          });
        }
        this.setState({ dataDeXuat: res.data.Data, loading: false });
      } else {
        this.setState({ loading: false });
        message.error(`${res.data && res.data.Message ? res.data.Message : "Lấy chi tiết đề xuất thất bại"}`);
      }
    });
  };

  handleInputChange = (name) => (event) => {
    // console.log(event);
    const { dataDeXuat } = this.state;
    dataDeXuat[name] = event.target ? event.target.value : event;
    this.setState({
      dataDeXuat,
    });
  };
  handleSelectChange = (name) => (value) => {
    const { dataDeXuat } = this.state;
    dataDeXuat[name] = value;
    this.setState({
      dataDeXuat,
    });
  };
  handleSelectMember = (index) => (value) => {
    const { dataDeXuat } = this.state;
    dataDeXuat.ThanhVienNghienCuu[index].CanBoID = value;
    this.setState({
      dataDeXuat,
    });
  };
  handleCloseModal = () => {
    this.onInitData();
    this.modal.destroy();
  };

  handleOpenModal = (name) => () => {
    switch (name) {
      case "AddFileModal":
        this.modal = Modal.confirm({
          icon: <i />,
          content: <AddFileModal NghiepVuID={this.Id} onClose={this.handleCloseModal} />,
        });
        break;

      default:
        break;
    }
  };

  onEditorDone = (name) => (value) => {
    const { dataDeXuat } = this.state;
    dataDeXuat[name] = value;
    this.setState({ dataDeXuat });
  };
  handleSubmit = () => {
    const {
      DeXuatID,
      MaDeXuat,
      TenDeXuat,
      LinhVucNghienCuu,
      LinhVucKinhTeXaHoi,
      CapQuanLy,
      NguoiDeXuat,
      TinhCapThiet,
      MucTieu,
      SanPham,
      KinhPhiDuKien,
      CoQuanID,
      ThoiGianNghienCuu,
      NgayDeXuat,
      NoiDung,
      DiaChiUngDung,
      ThoiGianThucHienTu,
      ThoiGianThucHienDen,
      ThuocChuongTrinh,
    } = this.state.dataDeXuat;
    const newData = {
      DeXuatID,
      MaDeXuat,
      TenDeXuat,
      LinhVucNghienCuu,
      LinhVucKinhTeXaHoi,
      CapQuanLy,
      NguoiDeXuat,
      TinhCapThiet,
      MucTieu,
      SanPham,
      KinhPhiDuKien,
      CoQuanID,
      ThoiGianNghienCuu,
      NgayDeXuat,
      NoiDung,
      DiaChiUngDung,
      ThoiGianThucHienTu,
      ThoiGianThucHienDen,
      ThuocChuongTrinh,
    };
    this.setState({ confirmLoading: true });
    api.capNhatDeXuat(newData).then((res) => {
      if (!res || !res.data || res.data.Status !== 1) {
        this.setState({ confirmLoading: false });
        message.error(`${res && res.data ? res.data.Message : "Lỗi hệ thống"}`);
      } else {
        message.success("Cập nhật thành công");

        this.isChanged = false;
        if (this.path) {
          this.props.history.push(this.path);
        } else {
          this.setState({ confirmLoading: false });
          this.onInitData();
        }
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
            this.onInitData();
            // this.props.onClose();
          }
        });
      },
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
        title: "Ghi chú",
        dataIndex: "NoiDung",
        key: "NoiDung",
      },
      {
        title: "File đính kèm",
        dataIndex: "files",
        key: "files",
        render: (text, record, index) => (
          <div>
            {record.files.map((item) => (
              <span className="border border-primary rounded mx-1 my-1 d-inline-block p-1">
                <a download={item.TenFileGoc} target="_blank" href={item.FileUrl}>
                  {item.TenFileGoc}
                </a>
                {this.props.role.edit ? <Icon onClick={() => this.onDeleteFile(item)} className="pointer ml-1" type="close"></Icon> : null}
              </span>
            ))}
          </div>
        ),
      },
    ];
    return <Table locale={{ emptyText: "Không có dữ liệu" }} pagination={false} rowKey="FileDinhKemID" bordered dataSource={data} columns={columns}></Table>;
  };

  renderThongTinXetDuyetTable = (data) => {
    const columns = [
      {
        title: "STT",
        dataIndex: "index",
        key: "index",
        render: (text, record, index) => <p>{index + 1}</p>,
      },
      {
        title: "Quyết định",
        dataIndex: "TrangThai",
        key: "TrangThai",
        render: (text, record, index) => {
          const quyetDinh = TrangThai.find((d) => d.value === text);
          return <p>{quyetDinh ? quyetDinh.label : ""}</p>;
        },
      },
      {
        title: "Cán bộ thực hiện",
        dataIndex: "TenNguoiThucHien",
        key: "TenNguoiThucHien",
      },
      {
        title: "Ngày thực hiện",
        dataIndex: "NgayThucHien",
        key: "NgayThucHien",
        render: (text, record, index) => <p>{moment(text).format("DD/MM/YYYY")}</p>,
      },
      {
        title: "Ghi chú",
        dataIndex: "NoiDung",
        key: "NoiDung",
      },
      {
        title: "File đính kèm",
        dataIndex: "FileXetDuyet",
        key: "FileXetDuyet",
        render: (text, record, index) => (
          <div>
            {record.FileXetDuyet.map((item) => (
              <span className="border border-primary rounded mx-1 my-1 d-inline-block p-1">
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
    return <Table locale={{ emptyText: "Không có dữ liệu" }} pagination={false} rowKey="DuyetDeTaiID" bordered dataSource={data} columns={columns}></Table>;
  };

  render() {
    const { role } = this.props;

    const { confirmLoading, dataDeXuat, canEdit } = this.state;

    if ([2, 4, 5, 6].includes(dataDeXuat.TrangThai)) {
      role.edit = 0;
    }

    if (canEdit) {
      role.edit = 1;
    }
    const { FileDinhKem } = dataDeXuat;
    const groupedFileDinhKem = convertFileTable(FileDinhKem, ["FileDinhKemID", "TenFileGoc", "FileUrl", "NgayTao", "TenNguoiTao", "NoiDung"]);
    this.isChanged = JSON.stringify(this.originalData) !== JSON.stringify(this.state.dataDeXuat);

    return (
      <LayoutWrapper>
        <Prompt
          when={this.isChanged}
          message={(location) => {
            if (location.pathname === "/") {
              return true;
            }
            if (this.isChanged) {
              this.path = location.pathname;
              Modal.confirm({
                content: "Bạn có muốn lưu dữ liệu thay đổi trước khi thoát ?",
                onOk: () => {
                  this.submitBtn.current.click();
                },
                onCancel: () => {
                  this.isChanged = false;
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
        <PageHeader>Chi tiết đề xuất</PageHeader>
        <Box>
          <Spin spinning={this.state.loading}>
            <div className="custom-collapse">
              <Collapse defaultActiveKey={["1", "2", "3"]} expandIconPosition="right">
                <Panel header="Thông tin đề xuất đề tài" key="1">
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
                      <div className="col-lg-6 col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-4 ">Mã đề xuất </div>
                          <div className="col-6 col-lg-8 ">
                            <GoInput
                              disabled={!role.edit}
                              withTextToInput
                              onChange={this.handleInputChange("MaDeXuat")}
                              value={dataDeXuat.MaDeXuat}
                              // validators={["required"]} errorMessages={["Nội dung bắt buộc"]}
                            />
                          </div>
                        </div>
                      </div>
                      <div className="col-lg-6 col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-4 ">Ngày đề xuất</div>
                          <div className="col-6 col-lg-8 ">
                            <GoDatePicker
                              disabled={!role.edit}
                              withTextToInput
                              onChange={(date, dateString) => {
                                const { dataDeXuat } = this.state;
                                dataDeXuat.NgayDeXuat = date ? date.format("YYYY-MM-DD") : null;
                                this.setState({ dataDeXuat });
                              }}
                              value={dataDeXuat.NgayDeXuat ? moment(dataDeXuat.NgayDeXuat, "YYYY-MM-DD") : dataDeXuat.NgayDeXuat}
                              format="DD/MM/YYYY"
                            />
                          </div>
                        </div>
                      </div>
                      <div className=" col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-2 ">
                            Tên đề xuất <span className="text-danger">*</span>
                          </div>
                          <div className="col-6 col-lg-10 ">
                            <GoInput
                              disabled={!role.edit}
                              withTextToInput
                              onChange={this.handleInputChange("TenDeXuat")}
                              value={dataDeXuat.TenDeXuat}
                              validators={["required"]}
                              errorMessages={["Nội dung bắt buộc"]}
                            />
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
                              allowClear
                              showSearch
                              onChange={this.handleSelectChange("LinhVucKinhTeXaHoi")}
                              value={dataDeXuat.LinhVucKinhTeXaHoi === 0 ? null : dataDeXuat.LinhVucKinhTeXaHoi}
                              //  validators={["required"]} errorMessages={["Nội dung bắt buộc"]}
                            />
                          </div>
                        </div>
                      </div>

                      <div className="col-lg-6 col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-4 ">
                            Cấp quản lý <span className="text-danger">*</span>
                          </div>
                          <div className="col-6 col-lg-8 ">
                            <TreeSelectWithApi
                              disabled={!role.edit}
                              withTextToInput
                              apiConfig={{
                                api: api.danhSachCayCapQuanly,
                                valueField: "ID",
                                nameField: "Name",
                              }}
                              value={dataDeXuat.CapQuanLy === 0 ? null : dataDeXuat.CapQuanLy}
                              onChange={this.handleSelectChange("CapQuanLy")}
                              validators={["required"]}
                              errorMessages={["Nội dung bắt buộc"]}
                            ></TreeSelectWithApi>
                          </div>
                        </div>
                      </div>
                      <div className="col-lg-6 col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-4 ">
                            Người đề xuất <span className="text-danger">*</span>
                          </div>
                          <div className="col-6 col-lg-8">
                            <SelectWithApi
                              disabled={!role.edit}
                              withTextToInput
                              apiConfig={{
                                api: api.DanhSachTaiKhoan,
                                valueField: "CanBoID",
                                nameField: "TenCanBo",
                                filter: {
                                  PageSize: 20000,
                                },
                              }}
                              useSearchAPI
                              value={dataDeXuat.NguoiDeXuat}
                              returnFullItem
                              onChange={(value, options) => {
                                const { dataDeXuat } = this.state;
                                dataDeXuat.CoQuanID = options ? options.CoQuanID : null;
                                dataDeXuat.NguoiDeXuat = value;
                                this.setState({
                                  dataDeXuat,
                                });
                              }}
                              validators={["required"]}
                              errorMessages={["Nội dung bắt buộc"]}
                            ></SelectWithApi>
                          </div>
                        </div>
                      </div>

                      <div className="col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-2 ">Tính cấp thiết</div>
                          <div className="col-10 ">
                            <GoEditor disabled={!role.edit} value={dataDeXuat.TinhCapThiet} onDone={this.onEditorDone("TinhCapThiet")}></GoEditor>
                          </div>
                        </div>
                      </div>
                      <div className="col-12 my-1">
                        <div className=" row align-items-center">
                          <div className=" col-lg-2">Mục tiêu </div>
                          <div className="col-lg-10 ">
                            <GoEditor disabled={!role.edit} value={dataDeXuat.MucTieu} onDone={this.onEditorDone("MucTieu")}></GoEditor>
                          </div>
                        </div>
                      </div>
                      <div className="col-12 my-1">
                        <div className=" row align-items-center">
                          <div className=" col-lg-2">Sản phẩm</div>
                          <div className="col-lg-10 ">
                            <GoEditor disabled={!role.edit} value={dataDeXuat.SanPham} onDone={this.onEditorDone("SanPham")}></GoEditor>
                          </div>
                        </div>
                      </div>

                      <div className="col-lg-6 col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-4 ">Kinh phí dự kiến</div>
                          <div className="col-6 col-lg-6 ">
                            <GoInput
                              disabled={!role.edit}
                              withTextToInput
                              isNumber
                              currency="VNĐ"
                              onChange={(value) => {
                                const { dataDeXuat } = this.state;
                                dataDeXuat.KinhPhiDuKien = value;
                                this.setState({ dataDeXuat });
                                // }
                              }}
                              value={dataDeXuat.KinhPhiDuKien}
                              validators={["isNumber", "isPositive"]}
                              errorMessages={["Kinh phí dự kiến phải là dạng số", "Kinh phí dự kiến phải lớn hơn 0"]}
                            />
                          </div>
                          <div className="col-2">đồng</div>
                        </div>
                      </div>
                      <div className="col-lg-6 col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-4 ">Thời gian nghiên cứu </div>
                          <div className="col-6 col-lg-8 ">
                            <GoSelect
                              withTextToInput
                              disabled={!role.edit}
                              onChange={this.handleInputChange("ThoiGianNghienCuu")}
                              value={dataDeXuat.ThoiGianNghienCuu}
                              data={[
                                {
                                  value: 12,
                                  text: "12 tháng",
                                },
                                {
                                  value: 18,
                                  text: "18 tháng",
                                },
                                {
                                  value: 24,
                                  text: "24 tháng",
                                },
                                {
                                  value: 36,
                                  text: "36 tháng",
                                },
                              ]}
                              //  validators={["isNumber", "isPositive"]} errorMessages={["Kinh phí dự kiến phải là dạng số", "Kinh phí dự kiến phải lớn hơn 0"]}
                            />
                          </div>
                        </div>
                      </div>

                      <div className="col-lg-6 col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-4 ">Thời gian thực hiện</div>
                          <div className="col-6 col-lg-8 ">
                            <div className="row justify-content-between align-items-center">
                              <div className="col-5">
                                <GoDatePicker
                                  monthPicker
                                  placeholder=""
                                  disabled={!role.edit}
                                  withTextToInput
                                  onChange={(date, dateString) => {
                                    const { dataDeXuat } = this.state;
                                    dataDeXuat.ThoiGianThucHienTu = date ? date.format("MM/YYYY") : null;
                                    this.setState({ dataDeXuat });
                                  }}
                                  value={dataDeXuat.ThoiGianThucHienTu ? moment(dataDeXuat.ThoiGianThucHienTu, "MM/YYYY") : dataDeXuat.ThoiGianThucHienTu}
                                  format="MM/YYYY"
                                />
                              </div>
                              <div className="col-2 px-0 text-center"> đến </div>
                              <div className="col-5">
                                <GoDatePicker
                                  monthPicker
                                  placeholder=""
                                  disabled={!role.edit}
                                  withTextToInput
                                  onChange={(date, dateString) => {
                                    const { dataDeXuat } = this.state;
                                    dataDeXuat.ThoiGianThucHienDen = date ? date.format("MM/YYYY") : null;
                                    this.setState({ dataDeXuat });
                                  }}
                                  value={dataDeXuat.ThoiGianThucHienDen ? moment(dataDeXuat.ThoiGianThucHienDen, "MM/YYYY") : dataDeXuat.ThoiGianThucHienDen}
                                  format="MM/YYYY"
                                />
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                      <div className=" col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-2 ">Nội dung</div>
                          <div className="col-6 col-lg-10  ">
                            <GoInput rows={3} isTextArea disabled={!role.edit} withTextToInput onChange={this.handleInputChange("NoiDung")} value={dataDeXuat.NoiDung} />
                          </div>
                        </div>
                      </div>
                      <div className="col-lg-6 col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-4 ">Thuộc chương trình</div>
                          <div className="col-6 col-lg-8 ">
                            <GoInput disabled={!role.edit} withTextToInput onChange={this.handleInputChange("ThuocChuongTrinh")} value={dataDeXuat.ThuocChuongTrinh} />
                          </div>
                        </div>
                      </div>
                      <div className="col-lg-6 col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-4 ">Địa chỉ ứng dụng</div>
                          <div className="col-6 col-lg-8 ">
                            <GoInput disabled={!role.edit} withTextToInput onChange={this.handleInputChange("DiaChiUngDung")} value={dataDeXuat.DiaChiUngDung} />
                          </div>
                        </div>
                      </div>
                      <div className="col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-12 mb-1">
                            File đính kèm
                            <div className="clearfix"></div>
                          </div>
                        </div>
                        <div>{this.renderFileTable(groupedFileDinhKem)}</div>
                        <div className="float-right mr-2 my-2">
                          <Button disabled={!role.edit} onClick={this.handleOpenModal("AddFileModal")} className="">
                            Thêm file đính kèm
                          </Button>
                        </div>
                      </div>
                      <div className=" col-12 text-right">
                        <button className="d-none" ref={this.submitBtn} type="submit">
                          submit
                        </button>
                        <div style={{ display: this.isChanged ? "block" : "none" }}>
                          <Button
                            loading={this.state.confirmLoading}
                            type="primary"
                            onClick={() => {
                              this.path = null;

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
                <Panel header="Thông tin xét duyệt đề tài" key="2">
                  <div>{this.renderThongTinXetDuyetTable(dataDeXuat.ThongTinXetDuyetDeTai)}</div>
                </Panel>
                <Panel header="Lịch sử chỉnh sửa" key="3">
                  <LichSuChinhSua data={dataDeXuat.ThongTinChinhSuaDeXuat}></LichSuChinhSua>
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
  const role = getRoleByKey2("ql-de-xuat");
  // console.log(role);
  state.QLDeXuat.role = role;
  return {
    ...state.QLDeXuat,
  };
}
export default connect(mapStateToProps, actions)(ChiTietDeXuat);
