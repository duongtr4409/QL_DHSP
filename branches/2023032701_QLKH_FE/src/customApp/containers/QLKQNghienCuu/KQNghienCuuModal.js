/**
 *
 * GoInput
 *
 */

import React from "react";
import { Modal } from "antd";
import Constants, { MODAL_NORMAL, ITEM_LAYOUT3, REQUIRED } from "../../../settings/constants";
import GoInput from "../../components/GoInput/index";
import GoSelect from "../../components/GoSelect/index";
import { ValidatorForm } from "react-form-validator-core";
// import PropTypes from 'prop-types';
// import styled from 'styled-components';

/* eslint-disable react/prefer-stateless-function */
class KQNghienCuuModal extends React.Component {
  constructor(props) {
    super(props);
    this.submitBtn = React.createRef();
    this.state = {
      confirmLoading: false,
      dataKQNC: {
        DeTaiKH: "",
        LoaiKQ: "",
        NoiDungKQNC: "",
        Files: [],
      },
      DSLoaiKQ: [
        {
          text: "Bài báo",
          value: "Baibao",
        },
      ],
    };
  }
  handleCancel = () => {
    this.props.onClose();
  };
  handleOk = () => {
    // this.setState({ confirmLoading: true });
    this.submitBtn.current.click();
  };
  handleSubmit = () => {};
  handleInputChange = (name) => (event) => {
    const { dataKQNC } = this.state;
    dataKQNC[name] = event.target.value;
    this.setState({
      dataKQNC,
    });
  };
  handleSelectChange = (name) => (value) => {
    const { dataKQNC } = this.state;
    dataKQNC[name] = value;
    this.setState({
      dataKQNC,
    });
  };
  render() {
    const { data } = this.props;
    const { confirmLoading, dataKQNC } = this.state;
    return (
      <Modal confirmLoading={confirmLoading} width={800} title={data ? "Thêm thông tin kết quả nghiên cứu" : "Sửa thông tin kết quả nghiên cứu"} visible={true} onOk={this.handleOk} onCancel={this.handleCancel}>
        <ValidatorForm ref="form" onSubmit={this.handleSubmit}>
          <div className="row justify-content-center align-items-center">
            <div className="col-12 col-lg-3 my-1">Đề tài khoa học</div>
            <div className="col-12 col-lg-9 my-1">
              <GoSelect onChange={this.handleSelectChange("DeTaiKH")} data={this.state.DSLoaiKQ} value={dataKQNC.DeTaiKH} validators={["required"]} errorMessages={["Nội dung bắt buộc"]} />
            </div>
            <div className="col-12 col-lg-3 my-1">Loại kết quả</div>
            <div className="col-12 col-lg-9 my-1">
              <GoSelect onChange={this.handleSelectChange("LoaiKQ")} data={this.state.DSLoaiKQ} value={dataKQNC.LoaiKQ} validators={["required"]} errorMessages={["Nội dung bắt buộc"]} />
            </div>
            <div className="col-12 col-lg-3 my-1">Nội dung kết quả nghiên cứu</div>
            <div className="col-12 col-lg-9 my-1">
              <GoInput onChange={this.handleInputChange("NoiDungKQNC")} value={dataKQNC.NoiDungKQNC} validators={["required"]} errorMessages={["Nội dung bắt buộc"]} />
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
