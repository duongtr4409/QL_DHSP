import React, {Component} from 'react';
import {Button, Form, Input, Modal,message} from 'antd';
import Constants, {LoaiThongTinNhaKhoaHoc} from "../../../settings/constants";

const {
  ITEM_LAYOUT3,
} = Constants;
const {Item} = Form;

const ModalAddEditQuaTrinhDaoTao = Form.create({name: 'modal_add_edit'})(
  class extends Component {
    constructor(props) {
      super(props);
      this.state = {};
    }

    onOk = (e) => {
      e.preventDefault();
      this.props.form.validateFields((err, value) => {
        if (!err) {
          if (value.KhoangThoiGian === "" && value.CoSoDaoTao === "" && value.ChuyenNganh === "" && value.HocVi === "") {
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
      let titleModal = "Thêm thông tin quá trình đào tạo";
      if (actions === 'edit') {
        titleModal = "Cập nhật thông tin quá trình đào tạo";
      }

      return (
        <Modal
          title={titleModal}
          width={600}
          onCancel={this.onCancel}
          visible={visible}
          footer={[
            <Button key="back" onClick={onCancel}>Hủy</Button>,
            <Button key="submit" htmlType="submit" type="primary" form="fromDT"
                    onClick={this.onOk} loading={loading} disabled={loading}>Lưu</Button>,
          ]}
        >
          <Form id="fromDT">
            {getFieldDecorator('CTNhaKhoaHocID', {initialValue: dataEdit.CTNhaKhoaHocID ? dataEdit.CTNhaKhoaHocID : null})}
            {getFieldDecorator('CanBoID', {initialValue: this.props.CanBoID})}
            {getFieldDecorator('CoQuanID', {initialValue: this.props.CoQuanID})}
            {getFieldDecorator('LoaiThongTin', {initialValue: LoaiThongTinNhaKhoaHoc.QuaTrinhDaoTao})}
            <Item label={"Khoảng thời gian"} {...ITEM_LAYOUT3}>
              {getFieldDecorator('KhoangThoiGian', {
                initialValue: dataEdit.KhoangThoiGian ? dataEdit.KhoangThoiGian : ""
              })(
                <Input autoFocus/>)}
            </Item>
            <Item label={"Tên cơ sở đào tạo"} {...ITEM_LAYOUT3}>
              {getFieldDecorator('CoSoDaoTao', {
                initialValue: dataEdit.CoSoDaoTao ? dataEdit.CoSoDaoTao : ""
              })(
                <Input/>)}
            </Item>
            <Item label={"Chuyên ngành"} {...ITEM_LAYOUT3}>
              {getFieldDecorator('ChuyenNganh', {
                initialValue: dataEdit.ChuyenNganh ? dataEdit.ChuyenNganh : ""
              })(
                <Input/>)}
            </Item>
            <Item label={"Học vị"} {...ITEM_LAYOUT3}>
              {getFieldDecorator('HocVi', {
                initialValue: dataEdit.HocVi ? dataEdit.HocVi : ""
              })(
                <Input/>)}
            </Item>
          </Form>
        </Modal>
      );
    }
  },
);
export {ModalAddEditQuaTrinhDaoTao}