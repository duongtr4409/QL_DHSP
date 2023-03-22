/**
 *
 * GoInput
 *
 */

import React from "react";
import { Modal, Icon, Button, Input, Divider, Upload, Avatar, message } from "antd";

import GoInput from "../../components/GoInput/index";
import GoSelect from "../../components/GoSelect/index";
import GoEditor from "../../components/GoEditor/editor";
import GoDatePicker from "../../components/GoDatePicker/index";
import { GoTreeSelect } from "../../components/index";
import { ValidatorForm } from "react-form-validator-core";
import ThemCanBoModal from "./ThemCanBoModal";
import api from "./config";
import lodash from "lodash";
import { apiDelete } from "../../../api";
import * as moment from "moment";
import TextArea from "antd/lib/input/TextArea";
import { withAPI } from "../../components/withAPI";
import { SelectWithApi } from "../../components/index";
import { checkFilesSize, checkFileType } from "../../../helpers/utility";

const TreeSelectWithApi = withAPI(GoTreeSelect);

// import PropTypes from 'prop-types';
// import styled from 'styled-components';

/* eslint-disable react/prefer-stateless-function */
class KQNghienCuuModal extends React.Component {
  modal = null;
  constructor(props) {
    super(props);
    this.submitBtn = React.createRef();
    this.state = {
      confirmLoading: false,
      dataDeXuat: {
        MaDeXuat: "",
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
        CoQuanID: "",
        FileDinhKem: [{ TenFileGoc: "", NoiDung: "", files: [] }],
      },
      DSLoaiKQ: [
        {
          text: "Bài báo",
          value: "Baibao",
        },
      ],
    };
  }
  componentDidMount() {
    const user = JSON.parse(localStorage.getItem("user"));
    if (user.CanBoID && user.CoQuanID !== 999999999) {
      const { dataDeXuat } = this.state;
      dataDeXuat.NguoiDeXuat = user.CanBoID;
      dataDeXuat.CoQuanID = user.CoQuanID;
      this.setState({ dataDeXuat });
    }
  }
  handleCancel = () => {
    this.props.onClose();
  };
  handleOk = () => {
    // this.setState({ confirmLoading: true });
    this.submitBtn.current.click();
  };
  handleSubmit = () => {
    this.setState({ confirmLoading: true });
    const { dataDeXuat } = this.state;
    const formData = new FormData();
    let files = [];
    lodash.cloneDeep(dataDeXuat.FileDinhKem).forEach((element, index) => {
      files = [...files, ...element.files];
      delete dataDeXuat.FileDinhKem[index].files;
    });

    files.forEach((element, index) => {
      formData.append("files", files[index]);
    });
    formData.append("ThongTinDeTai", JSON.stringify(dataDeXuat));

    api.themDeXuat(formData).then((res) => {
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
    this.modal.destroy();
  };
  handleAddPeople = () => {
    this.modal = Modal.confirm({
      icon: <i />,
      content: <ThemCanBoModal onClose={this.handleCloseModal}></ThemCanBoModal>,
    });
  };
  handleAddMember = () => {
    const { dataDeXuat } = this.state;
    dataDeXuat.ThanhVienNghienCuu.push({ CanBoID: null, VaiTro: 2, CoQuanID: null });
    this.setState({ dataDeXuat });
  };
  handleAddFileField = () => {
    const { dataDeXuat } = this.state;
    dataDeXuat.FileDinhKem.push({ TenFileGoc: "", NoiDung: "", files: [] });
    this.setState({ dataDeXuat });
  };
  handleChangeFileContent = (index) => (event) => {
    const { dataDeXuat } = this.state;
    dataDeXuat.FileDinhKem[index].NoiDung = event.target.value;
    this.setState({ dataDeXuat });
  };

  onEditorDone = (name) => (value) => {
    const { dataDeXuat } = this.state;
    dataDeXuat[name] = value;
    this.setState({ dataDeXuat });
  };

  render() {
    const { confirmLoading, dataDeXuat } = this.state;
    return (
      <Modal confirmLoading={confirmLoading} width={960} title={"Thêm thông tin đề xuất"} visible={true} onOk={this.handleOk} onCancel={this.handleCancel} okText="Lưu" cancelText="Huỷ">
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
                <div className=" col-lg-2 ">
                  Tên đề xuất <span className="text-danger">*</span>
                </div>
                <div className="col-6 col-lg-10 ">
                  <GoInput isTextArea onChange={this.handleInputChange("TenDeXuat")} value={dataDeXuat.TenDeXuat} validators={["required"]} errorMessages={["Nội dung bắt buộc"]} />
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
                    apiConfig={{
                      api: api.danhSachCayCapQuanly,
                      valueField: "ID",
                      nameField: "Name",
                    }}
                    value={dataDeXuat.CapQuanLy}
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
                    apiConfig={{
                      api: api.DanhSachTaiKhoan,
                      valueField: "CanBoID",
                      nameField: "TenCanBo",
                      filter: {
                        PageSize: 10000,
                      },
                    }}
                    returnFullItem
                    // localSearch
                    value={dataDeXuat.NguoiDeXuat}
                    onChange={(value, options) => {
                      // console.log(value, options);
                      const { dataDeXuat } = this.state;
                      dataDeXuat.CoQuanID = options ? options.CoQuanID : null;
                      dataDeXuat.NguoiDeXuat = value;
                      this.setState({
                        dataDeXuat,
                      });
                    }}
                    // hideCondition={{
                    //   field: "LaCanBoTrongTruong",
                    //   value: true,
                    // }}
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
                  <GoEditor c onDone={this.onEditorDone("TinhCapThiet")}></GoEditor>
                </div>
              </div>
            </div>
            <div className="col-12 my-1">
              <div className=" row align-items-center">
                <div className=" col-lg-2">Mục tiêu </div>
                <div className="col-lg-10 ">
                  <GoEditor onDone={this.onEditorDone("MucTieu")}></GoEditor>
                </div>
              </div>
            </div>
            <div className="col-12 my-1">
              <div className=" row align-items-center">
                <div className=" col-lg-2">Sản phẩm</div>
                <div className="col-lg-10 ">
                  <GoEditor onDone={this.onEditorDone("SanPham")}></GoEditor>
                </div>
              </div>
            </div>

            <div className="col-lg-6 col-12 my-1  ">
              <div className="row align-items-center">
                <div className="col-6 col-lg-4 ">Kinh phí dự kiến</div>
                <div className="col-6 col-lg-6 ">
                  <GoInput
                    isNumber
                    onChange={this.handleInputChange("KinhPhiDuKien")}
                    value={dataDeXuat.KinhPhiDuKien}
                    validators={["isNumber", "isPositive"]}
                    errorMessages={["Kinh phí dự kiến phải là dạng số", "Kinh phí dự kiến phải lớn hơn 0"]}
                  />
                </div>
                <div className="col-2 px-0">đồng</div>
              </div>
            </div>
            <div className="col-lg-6 col-12 my-1  ">
              <div className="row align-items-center">
                <div className="col-6 col-lg-4 ">Thời gian nghiên cứu </div>
                <div className="col-6 col-lg-8 ">
                  <GoSelect
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
                  <GoInput rows={3} isTextArea onChange={this.handleInputChange("NoiDung")} value={dataDeXuat.NoiDung} />
                </div>
              </div>
            </div>
            <div className="col-lg-6 col-12 my-1  ">
              <div className="row align-items-center">
                <div className="col-6 col-lg-4 ">Thuộc chương trình</div>
                <div className="col-6 col-lg-8 ">
                  <GoInput onChange={this.handleInputChange("ThuocChuongTrinh")} value={dataDeXuat.ThuocChuongTrinh} />
                </div>
              </div>
            </div>
            <div className="col-lg-6 col-12 my-1  ">
              <div className="row align-items-center">
                <div className="col-6 col-lg-4 ">Địa chỉ ứng dụng</div>
                <div className="col-6 col-lg-8 ">
                  <GoInput onChange={this.handleInputChange("DiaChiUngDung")} value={dataDeXuat.DiaChiUngDung} />
                </div>
              </div>
            </div>
            {this.state.dataDeXuat.FileDinhKem.map((item, index) => {
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
                          const fileType = await checkFileType(file);
                          if (!fileType.valid) {
                            message.error(`File đính kèm không hợp lệ. (Chỉ được đính kèm file: ${fileType.fileTypes})`);
                            return;
                          }

                          const result = await checkFilesSize(file);
                          if (file.status === "removed") {
                            const fileIndex = dataDeXuat.FileDinhKem[index].files.findIndex((d) => d.uid === file.uid);
                            dataDeXuat.FileDinhKem[index].files.splice(fileIndex, 1);
                            dataDeXuat.FileDinhKem[index].TenFileGoc = fileList.map((item) => item.name).join(";");
                            this.setState({ dataDeXuat });
                          } else {
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
                        {index === this.state.dataDeXuat.FileDinhKem.length - 1 ? (
                          <Button
                            type="button"
                            className="mx-2"
                            onClick={(e) => {
                              e.stopPropagation();
                              this.handleAddFileField();
                            }}
                          >
                            Thêm file đính kèm
                          </Button>
                        ) : null}
                      </Upload>
                    </div>
                  </div>
                  <div className=" row align-items-center my-2">
                    <div className=" col-lg-2">Ghi chú</div>
                    <div className="col-lg-10 ">
                      <TextArea value={dataDeXuat.FileDinhKem[index].NoiDung} onChange={this.handleChangeFileContent(index)}></TextArea>
                    </div>
                  </div>
                </div>
              );
            })}
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
