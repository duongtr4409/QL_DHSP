import React from "react";
import { Modal, Icon, Button, Input, Upload, message, Radio } from "antd";
import DatePicker from "../../../../../components/uielements/datePickerFormat";
import moment from "moment";
import api from "../../config";
import { ValidatorForm } from "react-form-validator-core";
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
        GhiChu: "",
        TenNoiDung: "",
        TrangThaiTienDo: null,
        KetQuaDatDuoc: "",
        NgayThucHien: moment().toString(),
      },
    };
  }
  componentDidMount() {
    if (this.props.data) {
      let { data } = this.state;
      data = this.props.data;
      if (!data.NgayThucHien) {
        data.NgayThucHien = moment().toString();
      }
      this.setState({ data });
    }
  }
  handleCancel = () => {
    this.props.onClose();
  };
  handleOk = () => {
    this.setState({ confirmLoading: true });

    const newData = this.state.data;
    if (!this.props.data) {
      newData.DeTaiID = Number(this.props.DeTaiID);
    }
    newData.LoaiThongTin = 1;

    api.chinhSuaThongTinChiTiet(newData).then((res) => {
      if (!res || !res.data || res.data.Status !== 1) {
        this.setState({ confirmLoading: false });
        message.error(`${res && res.data ? res.data.Message : "Lỗi hệ thống"}`);
      } else {
        message.success("Cập nhật thành công");
        this.props.onClose();
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

  render() {
    const { confirmLoading, data } = this.state;
    return (
      <Modal confirmLoading={confirmLoading} width={960} title={"Cập nhật tiến độ thực hiện"} visible={true} onOk={this.handleOk} onCancel={this.handleCancel} cancelText="Huỷ" okText="Lưu">
        <ValidatorForm ref="form" onSubmit={this.handleSubmit}>
          <div className="row">
            <div className="col-12 my-1  ">
              <div className="row align-items-center">
                <div className=" col-lg-2 ">Tên nội dung</div>
                <div className="col-lg-10 ">
                  <p>{data.TenNoiDung}</p>
                </div>
              </div>
            </div>
            <div className="col-12 my-1  ">
              <div className="row align-items-center">
                <div className=" col-lg-2 ">Trạng thái</div>
                <div className="col-lg-10 ">
                  <Radio.Group onChange={this.handleInputChange("TrangThaiTienDo")} value={this.state.data.TrangThaiTienDo}>
                    <Radio value={0}>Chưa thực hiện</Radio>
                    <Radio value={1}>Đang thực hiện</Radio>
                    <Radio value={2}>Đã thực hiện</Radio>
                  </Radio.Group>
                </div>
              </div>
            </div>
            <div className="col-12 my-1  ">
              <div className="row align-items-center">
                <div className=" col-lg-2 ">Ngày thực hiện</div>
                <div className="col-lg-10 ">
                  <DatePicker
                    onChange={(date) => {
                      const { data } = this.state;
                      data.NgayThucHien = date ? date.format("YYYY-MM-DD") : moment().format("YYYY-MM-DD");
                      this.setState({ data });
                    }}
                    placeholder=""
                    value={data.NgayThucHien ? moment(new Date(data.NgayThucHien), "YYYY-MM-DD") : null}
                    format={"DD/MM/YYYY"}
                  />
                </div>
              </div>
            </div>
            <div className="col-12 my-1  ">
              <div className="row align-items-center">
                <div className=" col-lg-2 ">Kết quả đạt được</div>
                <div className="col-lg-10 ">
                  <TextArea value={data.KetQuaDatDuoc} onChange={this.handleInputChange("KetQuaDatDuoc")} placeholder="" allowClear />
                </div>
              </div>
            </div>
            <div className="col-12 my-1  ">
              <div className="row align-items-center">
                <div className=" col-lg-2 ">Ghi chú</div>
                <div className="col-lg-10 ">
                  <TextArea value={data.GhiChu} onChange={this.handleInputChange("GhiChu")} placeholder="" allowClear />
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
