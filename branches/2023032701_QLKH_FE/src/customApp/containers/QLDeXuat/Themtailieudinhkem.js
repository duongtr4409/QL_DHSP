import React from "react";
import { Modal, Icon, Button, Input, Upload, message } from "antd";
import api from "./config";
import { ValidatorForm } from "react-form-validator-core";
import { checkFilesSize, checkFileType } from "../../../helpers/utility";
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
      FileDinhKem: { NoiDung: "", files: [] },
    };
  }
  handleCancel = () => {
    this.props.onClose();
  };
  handleOk = () => {
    this.setState({ confirmLoading: true });
    const formData = new FormData();
    this.state.FileDinhKem.files.forEach((element, index) => {
      formData.append("files", this.state.FileDinhKem.files[index]);
    });
    formData.append("NoiDung", JSON.stringify({ LoaiFile: 2, NghiepVuID: this.props.NghiepVuID, NoiDung: this.state.FileDinhKem.NoiDung }));

    api.themFileDinhKem(formData).then((res) => {
      if (!res || !res.data || res.data.Status !== 1) {
        this.setState({ confirmLoading: false });
        message.error(`${res && res.data ? res.data.Message : "Lỗi hệ thống"}`);
      } else {
        message.success("Thêm mới thành công");
        this.props.onClose();
      }
    });
  };
  handleSubmit = () => {};
  handleInputChange = (name) => (event) => {
    const { FileDinhKem } = this.state;
    FileDinhKem[name] = event.target ? event.target.value : event;
    this.setState({
      FileDinhKem,
    });
  };

  render() {
    const { data } = this.props;
    const { confirmLoading, dataDeXuat } = this.state;
    return (
      <Modal confirmLoading={confirmLoading} width={960} title={"Thêm tài liệu đính kèm"} visible={true} onOk={this.handleOk} onCancel={this.handleCancel} cancelText="Huỷ" okText="Lưu">
        <ValidatorForm ref="form" onSubmit={this.handleSubmit}>
          <div className="row">
            <div className="col-12 my-1">
              <div className=" row align-items-center">
                <div className=" col-lg-2">File đính kèm</div>
                <div className="col-lg-10 ">
                  <Upload
                    fileList={this.state.FileDinhKem.files}
                    multiple
                    beforeUpload={(file) => {
                      return false;
                    }}
                    onChange={async ({ file, fileList }) => {
                      const { FileDinhKem } = this.state;
                      if (file.status === "removed") {
                        const fileIndex = FileDinhKem.files.findIndex((d) => d.uid === file.uid);
                        FileDinhKem.files.splice(fileIndex, 1);
                        this.setState({ FileDinhKem });
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
                        FileDinhKem.files.push(file);
                        this.setState({ FileDinhKem });
                      }
                    }}
                  >
                    <Button>
                      <Icon type="upload" /> Chọn file
                    </Button>
                  </Upload>
                </div>
              </div>
            </div>
            <div className="col-12 my-1  ">
              <div className="row align-items-center">
                <div className=" col-lg-2 ">Ghi chú</div>
                <div className="col-lg-10 ">
                  <TextArea value={this.state.FileDinhKem.NoiDung} onChange={this.handleInputChange("NoiDung")} placeholder="" allowClear />
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
