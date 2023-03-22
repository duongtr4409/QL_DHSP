import React, {Component} from 'react';
import {Button, Form, Input, message, Modal,} from 'antd';
import Constants, {LoaiThongTinNhaKhoaHoc} from "../../../settings/constants";
import styled from "styled-components";

const {
  ITEM_LAYOUT3,
} = Constants;
const {Item} = Form;

const ModalAddEditGiaiThuongKhoaHoc = Form.create({name: 'modal_add_edit'})(
  class extends Component {
    constructor(props) {
      super(props);
      this.state = {};
    }

    onOk = (e) => {
      e.preventDefault();
      this.props.form.validateFields((err, value) => {
        if (!err) {
          if (value.TieuDe === "" && value.KhoangThoiGian === "") {
            message.destroy();
            message.warning('Không có thông tin được nhập')
            return;
          }
          const {onCreate} = this.props;
          onCreate(value);
        }
      });
    };

    componentDidMount() {

    }

    onCancel = () => {
      const {onCancel} = this.props;
      onCancel();
    };

    render() {
      const {visible, onCancel, form, dataEdit, loading, actions} = this.props;
      const {getFieldDecorator} = form;
      let titleModal = "Thêm thông tin khen thưởng";
      if (actions === 'edit') {
        titleModal = "Cập nhật thông tin khen thưởng";
      }

      const Wrapper = styled.div`
        .ant-form-item-label {
          line-height: 25px;
        }
      `;

      return (
        <Modal
          title={titleModal}
          width={600}
          onCancel={this.onCancel}
          visible={visible}
          footer={[
            <Button key="back" onClick={onCancel}>Hủy</Button>,
            <Button key="submit" htmlType="submit" type="primary" form="formgiaithuong"
                    onClick={this.onOk} loading={loading} disabled={loading}>Lưu</Button>,
          ]}
        >
          <Form id="formgiaithuong">
            {getFieldDecorator('CTNhaKhoaHocID', {initialValue: dataEdit.CTNhaKhoaHocID ? dataEdit.CTNhaKhoaHocID : null})}
            {getFieldDecorator('CanBoID', {initialValue: this.props.CanBoID})}
            {getFieldDecorator('CoQuanID', {initialValue: this.props.CoQuanID})}
            {getFieldDecorator('LoaiThongTin', {initialValue: LoaiThongTinNhaKhoaHoc.GiaiThuongKhoaHoc})}
            <Item label={"Hình thức và nội dung giải thưởng"} {...ITEM_LAYOUT3}>
              {getFieldDecorator('TieuDe', {
                initialValue: dataEdit.TieuDe ? dataEdit.TieuDe : ""
              })(
                <Input autoFocus/>)}
            </Item>
            <Item label={"Năm tặng thưởng"} {...ITEM_LAYOUT3}>
              {getFieldDecorator('KhoangThoiGian', {
                initialValue: dataEdit.KhoangThoiGian ? dataEdit.KhoangThoiGian : ""
              })(
                <Input/>)}
            </Item>
          </Form>
        </Modal>
      );
    }
  },
);
export {ModalAddEditGiaiThuongKhoaHoc}