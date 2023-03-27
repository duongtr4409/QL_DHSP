import React from "react";
import { Modal, Icon, Button, Input, Upload, message, Form, Radio, Select } from "antd";
import DatePicker from "../../../../../components/uielements/datePickerFormat";
import moment from "moment";
import api from "../../config";
import { ValidatorForm } from "react-form-validator-core";
import lodash from "lodash";
import { checkFilesSize } from "../../../../../helpers/utility";
const { TextArea } = Input;

// import PropTypes from 'prop-types';
// import styled from 'styled-components';

/* eslint-disable react/prefer-stateless-function */
class GoModal extends React.Component {
  constructor(props) {
    super(props);
    this.submitBtn = React.createRef();
    this.state = {
      confirmLoading: false,
      data: {
        NoiDungDanhGia: "",
        ThoiGianThucHien: null,
        files: [],
      },
      files: [],
      LoaiKetQua: null,
      FileDinhKemOld: [],
    };
  }
  componentDidMount() {
    if (this.props.data) {
      const data = { ...this.state.data, ...this.props.data };
      this.setState({ data: data, FileDinhKemOld: this.props.data.FileDinhKem });
      this.props.form.setFieldsValue({
        NgayNghiemThu: moment(this.props.data.NgayNghiemThu, "YYYY-MM-DD"),
        QuyetDinh: this.props.data.QuyetDinh,
      });
      this.setState({ LoaiKetQua: 1, FileDinhKemOld: this.props.data ? this.props.data.FileDinhKem : [] });
    }
  }
  componentWillReceiveProps(props) {
    if (props.data !== this.props.data || props.LoaiKetQua !== this.props.LoaiKetQua) {
      this.originData = props.data;
      if (props.LoaiKetQua === 1) {
        this.setState({ LoaiKetQua: 1, FileDinhKemOld: props.data ? props.data.FileDinhKem : [] });
        if (props.data) {
          this.props.form.setFieldsValue({
            NgayNghiemThu: moment(props.data.NgayNghiemThu, "YYYY-MM-DD"),
            QuyetDinh: props.data.QuyetDinh,
          });
        }
      }

      if (props.LoaiKetQua === 0) {
        this.setState({ LoaiKetQua: 0 });
        if (props.data) {
          this.props.form.setFieldsValue({
            NgayNghiemThu: moment(props.data.NgayNghiemThu, "YYYY-MM-DD"),
            QuyetDinh: props.data.QuyetDinh,
            LoaiNghiemThu: props.data.LoaiNghiemThu,
            XepLoai: props.data.XepLoai,
          });
          if (props.data.XepLoaiKhac) {
            this.props.form.setFieldsValue({ XepLoaiKhac: props.data.XepLoaiKhac });
          }
          this.setState({ FileDinhKemOld: props.data ? props.data.FileDinhKem : [] });
        }
      }
    }
  }
  handleCancel = () => {
    this.props.onClose();
  };
  handleOk = (values) => {
    this.setState({ confirmLoading: true });

    let newData = lodash.cloneDeep(values);
    if (!this.props.data) {
      newData.DeTaiID = Number(this.props.DeTaiID);
    }
    newData.LoaiThongTin = 6;
    newData.NgayNghiemThu = values.NgayNghiemThu.format("YYYY-MM-DD");
    if (this.props.data) {
      const { ChiTietDeTaiID, DeTaiID, LoaiKetQua } = this.props.data;
      newData = { ...newData, ChiTietDeTaiID, DeTaiID, LoaiKetQua };
    }
    // console.log(newData);
    // return;

    api.chinhSuaThongTinChiTiet(newData).then((res) => {
      if (!res || !res.data || res.data.Status !== 1) {
        this.setState({ confirmLoading: false });
        message.error(`${res && res.data ? res.data.Message : "Lỗi hệ thống"}`);
      } else {
        const formData = new FormData();
        this.state.files.forEach((element, index) => {
          formData.append("files", this.state.files[index]);
        });
        formData.append("NoiDung", JSON.stringify({ LoaiFile: 12, NghiepVuID: res.data.Data, NoiDung: "" }));

        api.themFileDinhKem(formData).then((res) => {
          if (!res || !res.data || res.data.Status !== 1) {
            this.setState({ confirmLoading: false });
            message.error(`${res && res.data ? res.data.Message : "Thêm dữ liệu thành công, tải file đính kèm thất bại"}`);
            this.props.form.resetFields();
            this.setState({ confirmLoading: false, files: [], FileDinhKemOld: [] });
            this.props.onClose();
          } else {
            this.props.form.resetFields();
            message.success(`${this.props.data ? "Cập nhật" : "Thêm mới"} thành công`);

            this.setState({ confirmLoading: false, files: [], FileDinhKemOld: [] });
            this.props.onClose();
          }
        });
      }
    });
  };
  //   handleSubmit = () => {};
  handleInputChange = (name) => (event) => {
    const { data } = this.state;
    data[name] = event.target ? event.target.value : event;
    this.setState({
      data,
    });
  };
  handleDeleteFile = (FileDinhKemID) => {
    Modal.confirm({
      content: "Bạn có muốn xoá file đính kèm này?",
      cancelText: "Hủy",
      okText: "Đồng ý",
      onOk: () => {
        api.xoaFileDinhKem({ FileDinhKemID }).then((res) => {
          if (!res || !res.data || res.data.Status !== 1) {
            this.setState({ confirmLoading: false });
            message.error(`${res && res.data ? res.data.Message : "Lỗi hệ thống"}`);
          } else {
            message.success("Xoá thành công");
            const { FileDinhKemOld } = this.state;
            FileDinhKemOld.splice(
              FileDinhKemOld.findIndex((d) => d.FileDinhKemID === FileDinhKemID),
              1
            );
            this.setState({ FileDinhKemOld });
          }
        });
      },
    });
  };

  render() {
    const { confirmLoading, data } = this.state;

    const { getFieldDecorator } = this.props.form;
    return (
      <Form
        id="KQDG"
        onSubmit={(e) => {
          e.preventDefault();
          this.props.form.validateFields((err, values) => {
            if (!err) {
              // console.log("Received values of form: ", values);
              this.handleOk(values);
            }
          });
        }}
        className="login-form"
        layout="vertical"
        labelCol={{
          xs: { span: 24 },
          md: { span: 6 },
          lg: { span: 4 },
        }}
        wrapperCol={{ xs: { span: 24 }, md: { span: 18 }, lg: { span: 20 } }}
      >
        {this.state.LoaiKetQua === null ? (
          <Form.Item label="Kết quả">
            {getFieldDecorator("LoaiKetQua", {
              rules: [{ required: true, message: "Nội dung bắt buộc!" }],
            })(
              <Radio.Group>
                <Radio value={0}>Nghiệm thu</Radio>
                <Radio value={1}>Thanh lý</Radio>
              </Radio.Group>
            )}
          </Form.Item>
        ) : (
          ""
        )}

        {this.state.LoaiKetQua === 0 || this.props.form.getFieldValue("LoaiKetQua") === 0 ? (
          <div>
            <Form.Item label="Xếp loại">
              {getFieldDecorator("XepLoai", {
                // rules: [{ required: true, message: "Please input your Password!" }],
                initialValue: 1,
              })(
                <Select>
                  <Select.Option value={0}>Không đạt</Select.Option>
                  <Select.Option value={1}>Đạt</Select.Option>
                  <Select.Option value={2}>Xuất sắc</Select.Option>
                  <Select.Option value={3}>Khác</Select.Option>
                </Select>
              )}
            </Form.Item>

            <Form.Item label="Xếp loại khác" className={(this.props.data && this.props.data.XepLoai === 3) || this.props.form.getFieldValue("XepLoai") === 3 ? "d-block" : "d-none"}>
              {getFieldDecorator("XepLoaiKhac", {
                rules: [{ required: this.props.form.getFieldValue("XepLoai") === 3, message: "Nội dung bắt buộc!" }],
              })(<Input />)}
            </Form.Item>

            <Form.Item label="Loại nghiệm thu">
              {getFieldDecorator("LoaiNghiemThu", {
                rules: [{ required: true, message: "Nội dung bắt buộc!" }],
              })(
                <Radio.Group>
                  <Radio value={0}>Nghiệm thu cơ sở</Radio>
                  <Radio value={1}>Nghiệm thu chính thức</Radio>
                </Radio.Group>
              )}
            </Form.Item>
          </div>
        ) : (
          ""
        )}
        <Form.Item label={`Ngày ${this.props.LoaiKetQua === 1 || this.props.form.getFieldValue("XepLoai") === 1 ? "thanh lý" : "nghiệm thu"}`}>
          {getFieldDecorator("NgayNghiemThu", {
            rules: [{ required: true, message: "Nội dung bắt buộc!" }],
            initialValue: null,
          })(
            <DatePicker
              format="DD/MM/YYYY"
              placeholder=""
              onChange={() => {
                this.setState({ changed: true });
              }}
            />
          )}
        </Form.Item>
        <Form.Item label="Quyết định">
          {getFieldDecorator("QuyetDinh", {
            // rules: [{ required: true, message: "Please input your Password!" }],
          })(
            <Input
              onChange={() => {
                this.setState({ changed: true });
              }}
            />
          )}
        </Form.Item>
        <Form.Item label="File đính kèm">
          <Upload
            fileList={this.state.files}
            multiple
            beforeUpload={(file) => {
              return false;
            }}
            onChange={async ({ file, fileList }) => {
              this.setState({ changed: true });
              const { files } = this.state;
              if (file.status === "removed") {
                const fileIndex = files.findIndex((d) => d.uid === file.uid);
                files.splice(fileIndex, 1);
                this.setState({ files });
                return;
              }
              const result = await checkFilesSize(file);
              if (!result.valid) {
                message.error(`File đính kèm phải nhỏ hơn ${result.limitFileSize} MB. (${file.name})`);
                return;
              }
              files.push(file);

              this.setState({ files });
            }}
          >
            <Button>
              <Icon type="upload" /> Chọn file
            </Button>
          </Upload>

          <ul className="my-1  d-block">
            {this.state.FileDinhKemOld.map((item) => (
              <li className="pointer ant-upload-list-item">
                <div className="ant-upload-list-item-info ">
                  <Icon type="paper-clip" className="anticon anticon-paper-clip"></Icon>
                  <span className=" ant-upload-list-item-name ">{item.TenFileGoc}</span>
                  <i className="ant-upload-list-item-card-actions " style={{ top: 0 }} onClick={() => this.handleDeleteFile(item.FileDinhKemID)}>
                    <Icon type="delete"></Icon>
                  </i>
                </div>

                {/* <div className="clearfix"></div> */}
              </li>
            ))}
          </ul>
        </Form.Item>

        <div className="text-right mb-2">
          {this.props.LoaiKetQua !== 1 || this.state.changed ? (
            <Button type="primary" htmlType="submit" className="login-form-button" loading={this.state.confirmLoading}>
              Lưu
            </Button>
          ) : (
            ""
          )}
        </div>
      </Form>
    );
  }
}
export default Form.create({ name: "normal_login" })(GoModal);
