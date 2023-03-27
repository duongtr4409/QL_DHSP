import React, { Component } from "react";
import { Button, Col, Form, Icon, Input, message, Modal, Radio, Row, TreeSelect, Upload, Checkbox } from "antd";
import Constants from "../../../settings/constants";
import moment from "moment";
import Select, { Option } from "../../../components/uielements/select";
import DatePicker from "../../../components/uielements/datePickerFormat";
import ImgCrop from "antd-img-crop";

const { ITEM_LAYOUT2, ITEM_LAYOUT_HALF2, COL_ITEM_LAYOUT_HALF2, COL_COL_ITEM_LAYOUT_RIGHT2, REQUIRED } = Constants;
const { Item } = Form;
const { Group } = Radio;

function getBase64(img, callback) {
  const reader = new FileReader();
  reader.addEventListener("load", () => callback(reader.result));
  reader.readAsDataURL(img);
}

function beforeUpload(file) {
  const isJpgOrPng = file.type === "image/jpeg" || file.type === "image/png";
  if (!isJpgOrPng) {
    message.error("Sai định dạng ảnh (JPG hoặc PNG)");
  }
  const isLt2M = file.size / 1024 / 1024 < 1;
  if (!isLt2M) {
    message.error("File ảnh phải nhỏ hơn 1MB");
  }
  return isJpgOrPng && isLt2M;
}

const ModalAddEdit = Form.create({ name: "modal_add_edit_taikhoan" })(
  class extends Component {
    constructor(props) {
      super(props);
      this.state = {
        AnhDaiDien: {
          FileUrl: "",
          FileData: null,
        },
      };
    }

    upperFirstLetter = (word) => {
      let text = word.split(" ");
      let res = [];
      for (let i = 0; i < text.length; i++) {
        let text2 = text[i].split("");
        text2[0] = text2[0].toUpperCase();
        text2 = text2.join("");
        res[res.length] = text2;
      }
      return res.join(" ");
    };

    onOk = (e) => {
      e.preventDefault();
      this.props.form.validateFields((err, value) => {
        if (!err) {
          const TenNguoiDung = this.props.form.getFieldValue("TenNguoiDung").toLowerCase();
          if (TenNguoiDung === "" || TenNguoiDung == undefined) {
            this.props.form.setFields({
              TenNguoiDung: {
                errors: [new Error("Thông tin bắt buộc")],
              },
            });
            return;
          }
          value.TenCanBo = this.upperFirstLetter(value.TenCanBo);

          if (value.NgaySinh === null || value.NgaySinh === "Invalid date") {
            value.NgaySinh = "";
          }

          const { onCreate } = this.props;
          onCreate(value, this.state.AnhDaiDien);
        }
      });
    };

    componentDidMount() {
      if (this.props.actions === "edit") {
        const { AnhDaiDien } = this.state;
        AnhDaiDien.FileUrl = this.props.dataEdit.AnhHoSo;
        this.setState({ AnhDaiDien });
      }
    }

    disabledDate(current) {
      // Can not select days after today and today
      return current && current > moment().endOf("day");
    }

    InputTen = (e) => {
      const key = e.charCode;
      if ((key === 32 && e.target.value[e.target.value.length - 1] === " ") || (key === 32 && e.target.value.length === 0)) {
        e.preventDefault();
      }
    };

    inputNumber = (e) => {
      const key = e.charCode;
      if (key < 48 || key > 57) {
        e.preventDefault();
      }
    };

    onCancel = () => {
      const { onCancel } = this.props;
      onCancel();
    };

    inputTaiKhoan = (e) => {
      const key = e.charCode;
      if (key === 32) {
        e.preventDefault();
      }
    };

    checkDuplicate = (e) => {
      const { actions, dataEdit } = this.props;
      const value = e.target.value.toLowerCase();
      const { DanhSachTaiKhoanAll } = this.props;
      const { setFields } = this.props.form;
      const data = DanhSachTaiKhoanAll.filter((item) => item.TenNguoiDung.toLowerCase() === value);
      if (data.length > 0) {
        if (actions === "edit") {
          if (data[0].TenNguoiDung === dataEdit.TenNguoiDung) {
            setFields({
              TenNguoiDung: {
                value: value,
              },
            });
          } else {
            if (value === "") {
              setFields({
                TenNguoiDung: {
                  errors: [new Error("Thông tin bắt buộc")],
                },
              });
              this.setState({ error: true });
            } else {
              setFields({
                TenNguoiDung: {
                  errors: [new Error("Tài khoản đã tồn tại")],
                },
              });
              this.setState({ error: true });
            }
          }
        } else {
          setFields({
            TenNguoiDung: {
              errors: [new Error("Tài khoản đã tồn tại")],
            },
          });
          this.setState({ error: true });
        }
      } else {
        setFields({
          TenNguoiDung: {
            value: value,
          },
        });
        this.setState({ error: false });
        const myRg = /[^0-9a-zA-Z_@.\s]/g; //Chỉ nhập kí tự chữ và số, @, ., _
        const validate = myRg.test(value);
        if (validate) {
          setFields({
            TenNguoiDung: {
              errors: [new Error("Tên đăng nhập không hợp lệ")],
            },
          });
          this.setState({ error: true });
        } else {
          setFields({
            TenNguoiDung: {
              value: value,
            },
          });
          this.setState({ error: false });
        }
      }
    };

    beforeUpload = (file, callback) => {
      const { FileLimit } = this.props;
      const isJpgOrPng = file.type === "image/jpeg" || file.type === "image/png";
      const isLt2M = file.size / 1024 / 1024 < FileLimit;
      if (!isJpgOrPng) {
        message.error("Sai định dạng ảnh (JPG hoặc PNG)");
      } else if (!isLt2M) {
        message.error(`File ảnh phải nhỏ hơn ${FileLimit}MB`);
      } else {
        this.getBase64(file, callback);
      }
      return false;
    };

    checkFileImage = (file) => {
      const { FileLimit } = this.props;
      const isJpgOrPng = file.type === "image/jpeg" || file.type === "image/png";
      const isLt2M = file.size / 1024 / 1024 < FileLimit;
      if (!isJpgOrPng) {
        message.error("Sai định dạng ảnh (JPG hoặc PNG)");
      }
      if (!isLt2M) {
        message.error(`File ảnh phải nhỏ hơn ${FileLimit}MB`);
      }
      return isJpgOrPng && isLt2M;
    };

    getBase64 = (file, callback) => {
      const reader = new FileReader();
      reader.addEventListener("load", () => callback(reader.result, file));
      reader.readAsDataURL(file);
    };

    genAnhDaiDien = (base64, file) => {
      const { AnhDaiDien } = this.state;
      AnhDaiDien.FileUrl = base64;
      AnhDaiDien.FileData = file;
      this.setState({ AnhDaiDien });
    };

    render() {
      const { visible, onCancel, form, dataEdit, loading, actions, DanhSachNhomNguoiDung } = this.props;
      const { AnhDaiDien } = this.state;
      const { getFieldDecorator } = form;
      const { error } = this.state;
      let titleModal = "Thêm thông tin tài khoản";
      if (actions === "edit") {
        titleModal = "Sửa thông tin tài khoản";
      }

      const uploadButton = (
        <div>
          <Icon type={this.state.loading ? "loading" : "plus"} />
          <div className="ant-upload-text">Tải ảnh lên</div>
        </div>
      );

      return (
        <Modal
          title={titleModal}
          width={1000}
          onCancel={this.onCancel}
          visible={visible}
          footer={[
            <Button key="back" onClick={onCancel}>
              Hủy
            </Button>,
            <Button key="submit" htmlType="submit" type="primary" form="myForm" onClick={this.onOk} loading={loading} disabled={loading || error}>
              Lưu
            </Button>,
          ]}
        >
          <Form id="myForm">
            {getFieldDecorator("CanBoID", { initialValue: actions === "edit" ? dataEdit.CanBoID : 0 })}
            <Item label="Ảnh đại diện" {...ITEM_LAYOUT2}>
              <ImgCrop grid rotate modalOk={"Cắt ảnh"} modalCancel={"Hủy"} modalTitle={"Chỉnh sửa hình ảnh"} maxZoom={5} beforeCrop={this.checkFileImage} shape={"round"}>
                <Upload name="avatar" listType="picture-card" className="avatar-uploader" showUploadList={false} beforeUpload={(file) => this.beforeUpload(file, this.genAnhDaiDien)} accept={".png, .jpg, .jpeg"}>
                  {AnhDaiDien.FileUrl !== "" ? <img src={AnhDaiDien.FileUrl} alt="avatar" style={{ width: "100%" }} /> : uploadButton}
                </Upload>
              </ImgCrop>
            </Item>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item
                  label={
                    <span>
                      Tên tài khoản <span style={{ color: "red" }}>*</span>
                    </span>
                  }
                  {...ITEM_LAYOUT_HALF2}
                >
                  {getFieldDecorator("TenNguoiDung", {
                    initialValue: dataEdit.TenNguoiDung ? dataEdit.TenNguoiDung : "",
                  })(<Input autoFocus onKeyPress={this.inputTaiKhoan} onChange={this.checkDuplicate} />)}
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                    <Item label={"Tên cán bộ"} {...ITEM_LAYOUT_HALF2}>
                      {getFieldDecorator("TenCanBo", {
                        initialValue: dataEdit.TenCanBo ? dataEdit.TenCanBo : "",
                        rules: [{ ...REQUIRED }],
                      })(<Input onKeyPress={this.InputTen} style={{ textTransform: "capitalize" }} />)}
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item label={"Ngày sinh"} {...ITEM_LAYOUT_HALF2} className={"datepicker"}>
                  {getFieldDecorator("NgaySinh", {
                    initialValue: dataEdit.NgaySinh ? dataEdit.NgaySinh : "",
                    // rules: [{ ...REQUIRED }],
                  })(<DatePicker format={"DD/MM/YYYY"} placeholder={""} style={{ width: "100%" }} disabledDate={this.disabledDate} />)}
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                    <Item label={"Giới tính"} {...ITEM_LAYOUT_HALF2}>
                      {getFieldDecorator("GioiTinh", {
                        initialValue: dataEdit.GioiTinh != undefined ? dataEdit.GioiTinh : 1,
                      })(
                        <Group>
                          <Radio value={1}>Nam</Radio>
                          <Radio value={0}>Nữ</Radio>
                        </Group>
                      )}
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item label={"Email"} {...ITEM_LAYOUT_HALF2}>
                  {getFieldDecorator("Email", {
                    initialValue: dataEdit.Email,
                    rules: [{ type: "email", message: "Không đúng định dạng email" }],
                  })(<Input />)}
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                    <Item label={"Điện thoại"} {...ITEM_LAYOUT_HALF2}>
                      {getFieldDecorator("DienThoai", {
                        initialValue: dataEdit.DienThoai ? dataEdit.DienThoai : "",
                      })(<Input onKeyPress={this.inputNumber} />)}
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>

            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                {/*<Item label={"Chỗ ở hiện tại"} {...ITEM_LAYOUT_HALF2}>*/}
                {/*  {getFieldDecorator("DiaChi", {*/}
                {/*    initialValue: dataEdit.DiaChi ? dataEdit.DiaChi : "",*/}
                {/*  })(<Input />)}*/}
                {/*</Item>*/}
                <Item label={"Phân quyền"} {...ITEM_LAYOUT_HALF2}>
                  {getFieldDecorator("ListNhomNguoiDungID", {
                    initialValue: dataEdit.ListNhomNguoiDungID ? dataEdit.ListNhomNguoiDungID : undefined,
                  })(
                    <Select allowClear={true} notFoundContent={"Không có dữ liệu"} placeholder={"Chọn nhóm người dùng"} showSearch optionFilterProp={"TenNhom"} mode={"multiple"}>
                      {DanhSachNhomNguoiDung.map((e) => {
                        return (
                          <Option value={e.NhomNguoiDungID} label={e.TenNhom} key={`${e.NhomNguoiDungID}`}>
                            {e.TenNhom}
                          </Option>
                        );
                      })}
                    </Select>
                  )}
                </Item>
              </Col>
              {/*<Col {...COL_ITEM_LAYOUT_HALF2}>*/}
              {/*  <Row>*/}
              {/*    <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>*/}
              {/*      <Item label={"Phân quyền"} {...ITEM_LAYOUT_HALF2}>*/}
              {/*        {getFieldDecorator("ListNhomNguoiDungID", {*/}
              {/*          initialValue: dataEdit.ListNhomNguoiDungID ? dataEdit.ListNhomNguoiDungID : undefined,*/}
              {/*        })(*/}
              {/*          <Select allowClear={true} notFoundContent={"Không có dữ liệu"} placeholder={"Chọn nhóm người dùng"} showSearch optionFilterProp={"TenNhom"} mode={"multiple"}>*/}
              {/*            {DanhSachNhomNguoiDung.map((e) => {*/}
              {/*              return (*/}
              {/*                <Option value={e.NhomNguoiDungID} label={e.TenNhom} key={`${e.NhomNguoiDungID}`}>*/}
              {/*                  {e.TenNhom}*/}
              {/*                </Option>*/}
              {/*              );*/}
              {/*            })}*/}
              {/*          </Select>*/}
              {/*        )}*/}
              {/*      </Item>*/}
              {/*    </Col>*/}
              {/*  </Row>*/}
              {/*</Col>*/}
            </Row>
          </Form>
        </Modal>
      );
    }
  }
);
export { ModalAddEdit };
