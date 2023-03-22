import React from "react";
import { Modal, Icon, Button, Input, Upload, message, DatePicker } from "antd";
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
        NoiDungDaLam: "",
        SanPham: "",
        files: [],
      },
      FileDinhKemOld: [],
    };
  }
  componentDidMount() {
    if (this.props.data) {
      const data = { ...this.state.data, ...this.props.data };
      this.setState({ data: data, FileDinhKemOld: this.props.data.FileDinhKem });
    }
  }
  handleCancel = () => {
    this.props.onClose();
  };
  handleOk = () => {
    this.setState({ confirmLoading: true });

    const newData = lodash.cloneDeep(this.state.data);
    if (!this.props.data) {
      newData.DeTaiID = Number(this.props.DeTaiID);
    }
    newData.LoaiThongTin = 3;
    delete newData.files;

    api.chinhSuaThongTinChiTiet(newData).then((res) => {
      if (!res || !res.data || res.data.Status !== 1) {
        this.setState({ confirmLoading: false });
        message.error(`${res && res.data ? res.data.Message : "Thêm thông tin sản phẩm thất bại"}`);
      } else {
        const formData = new FormData();
        this.state.data.files.forEach((element, index) => {
          formData.append("files", this.state.data.files[index]);
        });
        formData.append("NoiDung", JSON.stringify({ LoaiFile: 8, NghiepVuID: res.data.Data, NoiDung: this.state.data.NoiDungDaLam }));

        api.themFileDinhKem(formData).then((res) => {
          if (!res || !res.data || res.data.Status !== 1) {
            this.setState({ confirmLoading: false });
            message.error(`${res && res.data ? res.data.Message : "Thêm sản phẩm thành công, đính kèm file thất bại"}`);
            this.props.onClose();
          } else {
            message.success(`${this.props.data ? "Cập nhật" : "Thêm mới"} thành công`);
          }
          this.props.onClose();
        });
        // message.success("Cập nhật thành công");
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

    return (
      <Modal confirmLoading={confirmLoading} width={960} title={"Sản phẩm đề tài"} visible={true} onOk={this.handleOk} onCancel={this.handleCancel} cancelText="Huỷ" okText="Lưu">
        <ValidatorForm ref="form" onSubmit={this.handleSubmit}>
          <div className="row">
            <div className="col-12 my-1  ">
              <div className="row align-items-center">
                <div className=" col-lg-2 ">Nội dung đã làm</div>
                <div className="col-lg-10 ">
                  <TextArea value={data.NoiDungDaLam} onChange={this.handleInputChange("NoiDungDaLam")} placeholder="" allowClear />
                </div>
              </div>
            </div>
            <div className="col-12 my-1  ">
              <div className="row align-items-center">
                <div className=" col-lg-2 ">Sản phẩm</div>
                <div className="col-lg-10 ">
                  <TextArea value={data.SanPham} onChange={this.handleInputChange("SanPham")} placeholder="" allowClear />
                </div>
              </div>
            </div>
            <div className="col-12 my-1">
              <div className=" row align-items-center">
                <div className=" col-lg-2">File đính kèm</div>
                <div className="col-lg-10 ">
                  <Upload
                    fileList={this.state.data.files}
                    multiple
                    beforeUpload={(file) => {
                      return false;
                    }}
                    onChange={async ({ file, fileList }) => {
                      const { data } = this.state;
                      if (file.status === "removed") {
                        const fileIndex = data.files.findIndex((d) => d.uid === file.uid);
                        data.files.splice(fileIndex, 1);
                        this.setState({ data });
                        return;
                      }
                      const result = await checkFilesSize(file);
                      if (!result.valid) {
                        message.error(`File đính kèm phải nhỏ hơn ${result.limitFileSize} MB. (${file.name})`);
                        return;
                      }
                      data.files.push(file);

                      this.setState({ data });
                    }}
                  >
                    <Button>
                      <Icon type="upload" /> Chọn file
                    </Button>
                  </Upload>
                  {this.state.FileDinhKemOld.map((item) => (
                    <div className="mx-1 my-1 file-upload-item d-block">
                      <Icon type="paper-clip"></Icon>
                      <span className="mx-2">{item.TenFileGoc}</span>
                      <span className="float-right delete-icon pointer" onClick={() => this.handleDeleteFile(item.FileDinhKemID)}>
                        <Icon type="delete"></Icon>
                      </span>
                      <div className="clearfix"></div>
                    </div>
                  ))}
                </div>
              </div>
            </div>
          </div>
        </ValidatorForm>
      </Modal>
    );
  }
}
export default GoModal;
