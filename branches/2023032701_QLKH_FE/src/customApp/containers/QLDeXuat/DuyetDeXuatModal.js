import React from "react";
import { Modal, Icon, Button, Input, Divider, Upload, Radio, message, Spin } from "antd";

import GoInput from "../../components/GoInput/index";
import GoSelect from "../../components/GoSelect/index";
import GoEditor from "../../components/GoEditor/editor";
import { ValidatorForm } from "react-form-validator-core";
import { TrangThai } from "./config";
import moment from "moment";
import api from "./config";
import { getSystemConfig, checkFilesSize } from "../../../helpers/utility";
import DatePicker from "../../../components/uielements/datePickerFormat";
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
      duyetDeTaiData: {
        NgayThucHien: moment(),
        TrangThai: 4,
        NoiDung: "",
        Files: [],
      },
      loading: true,
      capquanly: null,
    };
  }
  componentDidMount() {
    api
      .chiTietCapQuanly({ id: this.props.dexuat.CapQuanLy })
      .then((res) => {
        console.log(res);

        this.setState({ loading: false, capquanly: res.data.Data.Type });
      })
      .catch((err) => {
        message.error("Lấy cấp quản lý thất bại");
      });
  }
  handleCancel = () => {
    this.props.onClose();
  };
  handleOk = () => {
    // this.setState({ confirmLoading: true });
    this.submitBtn.current.click();
  };
  handleSubmit = () => {
    const { NgayThucHien, TrangThai, NoiDung, Files } = this.state.duyetDeTaiData;
    this.setState({ confirmLoading: true }, () => {
      setTimeout(() => {
        api
          .capNhatTrangThaiDeXuat({
            DeXuatID: this.props.dexuat.DeXuatID,
            NgayThucHien: NgayThucHien,
            TrangThai,
            NoiDung,
          })
          .then((res) => {
            if (!res || !res.data || res.data.Status !== 1) {
              this.setState({ confirmLoading: false });
              message.error(`${res && res.data ? res.data.Message : "Lỗi hệ thống"}`);
            } else {
              const formData = new FormData();

              const NghiepVuID = res.data.Data;
              Files.forEach((element, index) => {
                formData.append("files", this.state.duyetDeTaiData.Files[index]);
              });
              formData.append("NoiDung", JSON.stringify({ LoaiFile: 5, NghiepVuID, NoiDung }));

              // this.setState({ confirmLoading: false });
              api.themFileDinhKem(formData).then((res) => {
                if (!res || !res.data || res.data.Status !== 1) {
                  message.error(`${res.data.Message || "Duyệt đề xuất thành công, tải file đính kèm thất bại"}`);
                  // this.setState({ confirmLoading: false });
                } else {
                  this.props.onClose();
                  message.success("Duyệt đề xuất thành công");
                  // this.setState({ confirmLoading: false });
                }
              });
            }
          });
      }, 500);
    });

    // console.log(duyetDeTaiData);
  };
  handleChangeFile = async ({ file, fileList }) => {
    const { duyetDeTaiData } = this.state;
    if (file.status === "removed") {
      const fileIndex = duyetDeTaiData.Files.findIndex((d) => d.uid === file.uid);
      duyetDeTaiData.Files.splice(fileIndex, 1);

      this.setState({ duyetDeTaiData });
    } else {
      const result = await checkFilesSize(file);
      if (!result.valid) {
        message.error(`File đính kèm phải nhỏ hơn ${result.limitFileSize} MB. (${file.name})`);
        return;
      }
      duyetDeTaiData.Files.push(file);
      this.setState({ duyetDeTaiData });
    }
  };
  render() {
    const { data } = this.props;
    const { confirmLoading, duyetDeTaiData } = this.state;
    return (
      <Modal confirmLoading={confirmLoading} width={960} title={"Duyệt đề xuất đề tài"} visible={true} onOk={this.handleOk} onCancel={this.handleCancel} okText="Duyệt" cancelText="Huỷ">
        <Spin spinning={this.state.loading}>
          <ValidatorForm ref="form" onSubmit={this.handleSubmit}>
            <div className="row">
              <div className="col-lg-6 col-12 my-1  ">
                <div className="row align-items-center">
                  <div className="col-6 col-lg-4 ">Ngày thực hiện</div>
                  <div className="col-6 col-lg-8 ">
                    <DatePicker
                      value={duyetDeTaiData.NgayThucHien ? moment(duyetDeTaiData.NgayThucHien, "YYYY-MM-DD") : null}
                      format="DD/MM/YYYY"
                      onChange={(date, dateString) => {
                        const { duyetDeTaiData } = this.state;
                        duyetDeTaiData.NgayThucHien = date ? date.format("YYYY-MM-DD") : null;

                        this.setState({ duyetDeTaiData });
                      }}
                      placeholder=""
                    ></DatePicker>
                  </div>
                </div>
              </div>

              <div className=" col-12 my-2  ">
                <div className="row align-items-center">
                  <div className="col-6 col-lg-2 ">Quyết định</div>
                  <div className="col-6 col-lg-10 ">
                    <Radio.Group
                      onChange={(event) => {
                        const { duyetDeTaiData } = this.state;
                        duyetDeTaiData.TrangThai = event.target.value;
                        this.setState({ duyetDeTaiData });
                      }}
                      value={duyetDeTaiData.TrangThai}
                    >
                      {TrangThai.filter((d) => d.isDicision).map((item) => {
                        if (item.value === 6 && this.state.capquanly !== 2 && this.state.capquanly !== 3) return null;
                        return (
                          <Radio value={item.value} key={item.value}>
                            {item.label}
                          </Radio>
                        );
                      })}
                    </Radio.Group>
                  </div>
                </div>
              </div>
              <div className="col-12 my-1">
                <div className=" row align-items-center">
                  <div className=" col-lg-2">File đính kèm</div>
                  <div className="col-lg-10 ">
                    <Upload
                      fileList={this.state.duyetDeTaiData.Files}
                      multiple
                      beforeUpload={(file) => {
                        return false;
                      }}
                      action={(file) => {}}
                      onChange={this.handleChangeFile}
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
                    <TextArea
                      onChange={(event) => {
                        const { duyetDeTaiData } = this.state;
                        duyetDeTaiData.NoiDung = event.target.value;
                        this.setState({ duyetDeTaiData });
                      }}
                      allowClear
                    />
                  </div>
                </div>
              </div>
            </div>
            <button className="d-none" ref={this.submitBtn} type="submit">
              submit
            </button>
          </ValidatorForm>
        </Spin>
      </Modal>
    );
  }
}
export default GoModal;
