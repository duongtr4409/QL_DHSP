import React from "react";
import { Modal, Icon, Button, InputNumber, Upload, message, DatePicker, Input } from "antd";
import moment from "moment";
import api from "../../config";
import { ValidatorForm } from "react-form-validator-core";
import { GoInput } from "../../../../components";
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
        TongKinhPhiDuocDuyet: 0,
        TienDoCap: 0,
        TienDoQuyetToan: 0,
      },
    };
  }
  componentDidMount() {
    if (this.props.data) {
      this.setState({ data: this.props.data });
    }
  }
  handleCancel = () => {
    this.props.onClose();
  };
  handleOk = () => {
    this.submitBtn.current.click();
  };
  handleSubmit = () => {
    this.setState({ confirmLoading: true });

    const newData = this.state.data;
    if (!this.props.data) {
      newData.DeTaiID = Number(this.props.DeTaiID);
    }
    newData.LoaiThongTin = 2;

    api.chinhSuaThongTinChiTiet(newData).then((res) => {
      if (!res || !res.data || res.data.Status !== 1) {
        this.setState({ confirmLoading: false });
        message.error(`${res && res.data ? res.data.Message : "Lỗi hệ thống"}`);
      } else {
        message.success(`${this.props.data ? "Cập nhật" : "Thêm mới"} thành công`);
        this.props.onClose();
      }
    });
  };
  //   handleSubmit = () => {};
  handleInputChange = (name) => (event) => {
    const { data } = this.state;
    data[name] = event && event.target ? event.target.value : event;
    this.setState({
      data,
    });
  };

  render() {
    const { confirmLoading, data } = this.state;
    return (
      <Modal confirmLoading={confirmLoading} width={480} title={"Thông tin kinh phí"} visible={true} onOk={this.handleOk} onCancel={this.handleCancel} cancelText="Huỷ" okText="Lưu">
        <ValidatorForm ref="form" onSubmit={this.handleSubmit}>
          <div className="row">
            <div className="col-12 my-1  ">
              <div className="row align-items-center">
                <div className=" col-12 col-lg-6 ">Tổng kinh phí được duyệt</div>
                <div className="col-12 col-lg-6 ">
                  <GoInput isNumber onChange={this.handleInputChange("TongKinhPhiDuocDuyet")} value={data.TongKinhPhiDuocDuyet} validators={["isNumber", "isPositive"]} errorMessages={["Dữ liệu nhập phải là dạng số", "Dữ liệu nhập phải lớn hơn 0"]} />
                  {/* <InputNumber value={data.TongKinhPhiDuocDuyet} onChange={this.handleInputChange("TongKinhPhiDuocDuyet")} placeholder="" allowClear /> */}
                </div>
              </div>
            </div>
            <div className="col-12 my-1  ">
              <div className="row align-items-center">
                <div className=" col-12 col-lg-6 ">Tiến độ cấp</div>
                <div className="col-12 col-lg-6 ">
                  <GoInput isNumber onChange={this.handleInputChange("TienDoCap")} value={data.TienDoCap} validators={["isNumber", "isPositive"]} errorMessages={["Dữ liệu nhập phải là dạng số", "Dữ liệu nhập phải lớn hơn 0"]} />
                  {/* <InputNumber value={data.TienDoCap} onChange={this.handleInputChange("TienDoCap")} placeholder="" allowClear /> */}
                </div>
              </div>
            </div>
            <div className="col-12 my-1  ">
              <div className="row align-items-center">
                <div className=" col-12 col-lg-6 ">Tiến độ quyết toán</div>
                <div className="col-12 col-lg-6 ">
                  <GoInput isNumber onChange={this.handleInputChange("TienDoQuyetToan")} value={data.TienDoQuyetToan} validators={["isNumber", "isPositive"]} errorMessages={["Dữ liệu nhập phải là dạng số", "Dữ liệu nhập phải lớn hơn 0"]} />
                  {/* <InputNumber value={data.TienDoQuyetToan} onChange={this.handleInputChange("TienDoQuyetToan")} placeholder="" allowClear /> */}
                </div>
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
export default GoModal;
