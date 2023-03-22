import React, {Component} from "react";
import Constants from "../../../settings/constants";
import {Modal, Form, Input, Radio, Row, Col, Button} from "antd";
import Select, {Option} from "../../../components/uielements/select";

const {Item} = Form;
const {MODAL_NORMAL, ITEM_LAYOUT3, REQUIRED} = Constants;

const ModalAddEdit = Form.create({name: "modal_add_adTemplate"})(
  // eslint-disable-next-line
  class extends Component {
    onOk = e => {
      e.preventDefault();
      this.props.form.validateFields((err, value) => {
        if (!err) {
          const {onCreate} = this.props;
          onCreate(value);
        }
      });
    };

    render() {
      const {visible, onCancel, form, loading, dataEdit, actions} = this.props;
      const {getFieldDecorator} = form;
      return (
        <Modal
          title={dataEdit.ThamSoID ? "Sửa tham số hệ thống" : "Thêm mới tham số hệ thống"}
          okText="Lưu"
          cancelText="Hủy"
          width={MODAL_NORMAL}
          visible={visible}
          onCancel={onCancel}
          footer={[
            <Button key="back" onClick={onCancel}>Hủy</Button>,
            <Button key="submit" htmlType="submit" type="primary" form="myForm"
                    onClick={this.onOk} loading={loading} disabled={loading}>Lưu</Button>,
          ]}
        >
          <Form layout="horizontal">
            {getFieldDecorator('SystemConfigID', {initialValue: dataEdit.SystemConfigID ? dataEdit.SystemConfigID : null})}
            <Item label="Tham số" {...ITEM_LAYOUT3}>
              {getFieldDecorator("ConfigKey", {
                initialValue: dataEdit.ConfigKey ? dataEdit.ConfigKey : "",
                rules: [{...REQUIRED}]
              })(<Input placeholder={"Nhập tên tham số"} autoFocus={true}
                //disabled={actions === 'edit'}
              />)}
            </Item>
            <Item label="Giá trị" {...ITEM_LAYOUT3}>
              {getFieldDecorator("ConfigValue", {
                initialValue: dataEdit.ConfigValue
                  ? dataEdit.ConfigValue
                  : dataEdit.ConfigKey && dataEdit.ConfigKey === 'PAGE_SIZE' ? undefined : "",
                rules: [{...REQUIRED}]
              })(dataEdit.ConfigKey && dataEdit.ConfigKey === 'PAGE_SIZE' ?
                <Select>
                  <Option value={10}>10</Option>
                  <Option value={20}>20</Option>
                  <Option value={30}>30</Option>
                  <Option value={40}>40</Option>
                </Select> :
                <Input placeholder={"Nhập giá trị"}/>)}
            </Item>
            <Item label="Ghi chú" {...ITEM_LAYOUT3}>
              {getFieldDecorator("Description", {
                initialValue: dataEdit.Description
                  ? dataEdit.Description
                  : ""
              })(<textarea style={{width:'100%'}} placeholder={'Nhập ghi chú'}/>)}
            </Item>
          </Form>
        </Modal>
      );
    }
  }
);
export {ModalAddEdit};
