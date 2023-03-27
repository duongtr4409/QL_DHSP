import React, {Component} from 'react';
import {ITEM_LAYOUT3, REQUIRED} from '../../../settings/constants';
import {Modal, Form, Input, Button, Checkbox, Row, Col, Radio} from 'antd';
import TreeSelectEllipsis from "../../components/TreeSelectEllipsis";

const {Item} = Form;

const ModalAddEdit = Form.create({name: 'modal_add_edit'})(
  // eslint-disable-next-line
  class extends Component {
    constructor(props) {
      super(props);
      this.state = {
        checkDangSuDung: true,
        Status: true,
        isAddNew: true,
        ListNhiemVuSelected: [],
        ListNameNhiemVu: []
      };
    }

    componentDidMount() {
      const {dataEdit, actions} = this.props;
      let Status = true;
      if (actions < 3) {
        Status = true;
      } else {
        Status = dataEdit.item.Status;
      }
      this.setState({Status})
    }

    onOk = (e) => {
      const {actions} = this.props;
      e.preventDefault();
      this.props.form.validateFields((err, value) => {
        if (!err) {
          value.Status = this.state.Status;
          const {onCreate} = this.props;
          if (actions === 2) {
            value.isAddNew = this.state.isAddNew;
            if (!value.isAddNew) {
              value.ListNhiemVuSelected = [];
              for (let i = 0; i < this.state.ListNhiemVuSelected.length; i++) {
                value.ListNhiemVuSelected.push({
                  Name: this.state.ListNameNhiemVu[i].props.title,
                  Id: this.state.ListNhiemVuSelected[i]
                })
              }
            }
          }
          // console.log(value);
          onCreate(value);
        }
      });
    };

    render() {
      const {visible, onCancel, form, dataEdit, loading, actions, DanhSachNhiemVu} = this.props;
      const {getFieldDecorator} = form;
      const {Status, isAddNew, ListNhiemVuSelected} = this.state;
      const {parentNode, item} = dataEdit;
      return (
        <Modal
          title={actions === 3 ? "Sửa thông tin loại kết quả" : "Thêm thông tin loại kết quả"}
          width={600}
          visible={visible}
          onCancel={onCancel}
          footer={[
            <Button key="back" onClick={onCancel}>Hủy</Button>,
            <Button key="submit" htmlType="submit" type="primary" form="myForm"
                    onClick={this.onOk} loading={loading}>Lưu</Button>,
          ]}
        >
          <Form id="myForm" layout="horizontal">
            {getFieldDecorator('Id', {initialValue: actions === 3 ? item.Id : null})}
            {getFieldDecorator('MappingId', {initialValue: actions === 3 ? item.MappingId : null})}
            {actions === 2 ? <Item label={'Phân loại'} {...ITEM_LAYOUT3}>
              <Radio.Group
                onChange={(e) => {
                  this.setState({isAddNew: e.target.value});
                }}
                value={this.state.isAddNew}
              >
                <Radio value={true}>Thêm mới</Radio>
                <Radio value={false}>Chọn từ danh mục hệ thống</Radio>
              </Radio.Group>
            </Item> : ""}
            {actions !== 2 ? <Item label={'Tên loại kết quả'} {...ITEM_LAYOUT3}>
              {getFieldDecorator('Name', {
                initialValue: actions === 3 ? item.Name : "",
                rules: [{...REQUIRED}]
              })(
                <Input/>
              )}
            </Item> : ""}
            {actions === 2 && !isAddNew ? <Item label={'Chọn loại kết quả'} {...ITEM_LAYOUT3}>
              <TreeSelectEllipsis
                showSearch
                data={DanhSachNhiemVu}
                defaultValue={undefined}
                dropdownStyle={{maxHeight: 400, overflowX: 'hidden', maxWidth: 500}}
                placeholder=""
                allowClear
                notFoundContent={"Không có dữ liệu"}
                treeNodeFilterProp={'label'}
                multiple
                value={ListNhiemVuSelected}
                onChange={(value, options) => this.setState({ListNhiemVuSelected: value, ListNameNhiemVu: options})}
                disabledCategoryId={true}
              />
            </Item> : ""}
            {actions === 2 && isAddNew ? <Item label={'Tên loại kết quả'} {...ITEM_LAYOUT3}>
              {getFieldDecorator('Name')(<Input/>)}
            </Item> : ""}
            {parentNode ? <Item label={'Loại kết quả cha'} {...ITEM_LAYOUT3}>
              {getFieldDecorator('ParentId', {initialValue: parentNode.Id})}
              <Input value={parentNode.Name} disabled/>
            </Item> : ""}
            <Item label={'Đang sử dụng'} {...ITEM_LAYOUT3}>
              <Checkbox checked={Status} onChange={value => this.setState({Status: value.target.checked})}/>
            </Item>
          </Form>
        </Modal>
      );
    }
  },
);
export {ModalAddEdit}