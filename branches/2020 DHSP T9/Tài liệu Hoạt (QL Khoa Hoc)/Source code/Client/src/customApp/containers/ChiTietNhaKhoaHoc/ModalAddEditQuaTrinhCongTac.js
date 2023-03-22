import React, {Component} from 'react';
import {Button, Form, Input, message, Modal,} from 'antd';
import Constants, {LoaiThongTinNhaKhoaHoc} from "../../../settings/constants";

const {
  ITEM_LAYOUT3,
} = Constants;
const {Item} = Form;

const ModalAddEditQuaTrinhCongTac = Form.create({name: 'modal_add_edit'})(
  class extends Component {
    constructor(props) {
      super(props);
      this.state = {};
    }

    onOk = (e) => {
      e.preventDefault();
      this.props.form.validateFields((err, value) => {
        if (!err) {
          if (value.KhoangThoiGian === "" && value.CoQuanCongTac === "" && value.DiaChiDienThoai === "" && value.ChucVu === "") {
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
      let titleModal = "Thêm thông tin quá trình công tác";
      if (actions === 'edit') {
        titleModal = "Cập nhật thông tin quá trình công tác";
      }

      return (
        <Modal
          title={titleModal}
          width={600}
          onCancel={this.onCancel}
          visible={visible}
          footer={[
            <Button key="back" onClick={onCancel}>Hủy</Button>,
            <Button key="submit" htmlType="submit" type="primary" form="formcongtac"
                    onClick={this.onOk} loading={loading} disabled={loading}>Lưu</Button>,
          ]}
        >
          <Form id="formcongtac">
            {getFieldDecorator('CTNhaKhoaHocID', {initialValue: dataEdit.CTNhaKhoaHocID ? dataEdit.CTNhaKhoaHocID : null})}
            {getFieldDecorator('CanBoID', {initialValue: this.props.CanBoID})}
            {getFieldDecorator('CoQuanID', {initialValue: this.props.CoQuanID})}
            {getFieldDecorator('LoaiThongTin', {initialValue: LoaiThongTinNhaKhoaHoc.QuaTrinhCongTac})}
            <Item label={"Khoảng thời gian"} {...ITEM_LAYOUT3}>
              {getFieldDecorator('KhoangThoiGian', {
                initialValue: dataEdit.KhoangThoiGian ? dataEdit.KhoangThoiGian : ""
              })(
                <Input autoFocus/>)}
            </Item>
            <Item label={"Cơ quan công tác"} {...ITEM_LAYOUT3}>
              {getFieldDecorator('CoQuanCongTac', {
                initialValue: dataEdit.CoQuanCongTac ? dataEdit.CoQuanCongTac : ""
              })(
                <Input/>)}
            </Item>
            <Item label={"Địa chỉ và điện thoại"} {...ITEM_LAYOUT3}>
              {getFieldDecorator('DiaChiDienThoai', {
                initialValue: dataEdit.DiaChiDienThoai ? dataEdit.DiaChiDienThoai : ""
              })(
                <Input/>)}
            </Item>
            <Item label={"Chức vụ"} {...ITEM_LAYOUT3}>
              {getFieldDecorator('ChucVu', {
                initialValue: dataEdit.ChucVu ? dataEdit.ChucVu : ""
              })(
                <Input/>)}
            </Item>
          </Form>
        </Modal>
      );
    }
  },
);
export {ModalAddEditQuaTrinhCongTac}