import React, {Component} from 'react';
import {Button, Form, Input, Modal, Row, Col, message} from 'antd';
import Constants, {LoaiThongTinNhaKhoaHoc} from "../../../settings/constants";
import DatePicker from "../../../components/uielements/datePickerFormat";

const {
  ITEM_LAYOUT2, ITEM_LAYOUT_HALF2,
  COL_ITEM_LAYOUT_HALF2,
  COL_COL_ITEM_LAYOUT_RIGHT2,
} = Constants;
const {Item} = Form;

const ModalAddEditVanBangChungChi = Form.create({name: 'modal_add_edit'})(
  class extends Component {
    constructor(props) {
      super(props);
      this.state = {};
    }

    onOk = (e) => {
      e.preventDefault();
      this.props.form.validateFields((err, value) => {
        if (!err) {
          if (value.TieuDe === "" && (value.NgayCap === "" || value.NgayCap == null) && value.SoHieu === "" && value.TrinhDo === "" && value.NoiCap === "") {
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
      let titleModal = "Thêm thông tin văn bằng chứng chỉ";
      if (actions === 'edit') {
        titleModal = "Cập nhật thông tin văn bằng chứng chỉ";
      }
      return (
        <Modal
          title={titleModal}
          width={700}
          onCancel={this.onCancel}
          visible={visible}
          footer={[
            <Button key="back" onClick={onCancel}>Hủy</Button>,
            <Button key="submit" htmlType="submit" type="primary" form="formvanbang"
                    onClick={this.onOk} loading={loading} disabled={loading}>Lưu</Button>,
          ]}
        >
          <Form id="formvanbang">
            {getFieldDecorator('CTNhaKhoaHocID', {initialValue: dataEdit.CTNhaKhoaHocID ? dataEdit.CTNhaKhoaHocID : null})}
            {getFieldDecorator('CanBoID', {initialValue: this.props.CanBoID})}
            {getFieldDecorator('CoQuanID', {initialValue: this.props.CoQuanID})}
            {getFieldDecorator('LoaiThongTin', {initialValue: LoaiThongTinNhaKhoaHoc.VanBangChungChi})}
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item label={"Tiêu đề"} {...ITEM_LAYOUT_HALF2}>
                  {getFieldDecorator('TieuDe', {
                    initialValue: dataEdit.TieuDe ? dataEdit.TieuDe : ""
                  })(<Input autoFocus/>)}
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                    <Item label={"Ngày cấp"} {...ITEM_LAYOUT_HALF2}>
                      {getFieldDecorator('NgayCap', {
                        initialValue: dataEdit.NgayCap ? dataEdit.NgayCap : null
                      })(<DatePicker placeholder={''}/>)}
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item label={"Số hiệu"} {...ITEM_LAYOUT_HALF2}>
                  {getFieldDecorator('SoHieu', {
                    initialValue: dataEdit.SoHieu ? dataEdit.SoHieu : ""
                  })(<Input/>)}
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                    <Item label={"Level"} {...ITEM_LAYOUT_HALF2}>
                      {getFieldDecorator('TrinhDo', {
                        initialValue: dataEdit.TrinhDo ? dataEdit.TrinhDo : ""
                      })(<Input/>)}
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item label={"Nơi cấp"} {...ITEM_LAYOUT_HALF2}>
                  {getFieldDecorator('NoiCap', {
                    initialValue: dataEdit.NoiCap ? dataEdit.NoiCap : ""
                  })(<Input/>)}
                </Item>
              </Col>
            </Row>
          </Form>
        </Modal>
      );
    }
  },
);
export {ModalAddEditVanBangChungChi}