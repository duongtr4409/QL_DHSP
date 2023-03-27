import React, {Component} from 'react';
import {Button, Col, Form, Icon, Input, message, Modal, Row, Upload, Checkbox} from 'antd';
import Constants from "../../../settings/constants";
import Select, {Option} from "../../../components/uielements/select";

const {
  ITEM_LAYOUT2,
  ITEM_LAYOUT_HALF2,
  COL_ITEM_LAYOUT_HALF2,
  COL_COL_ITEM_LAYOUT_RIGHT2,
  REQUIRED,
} = Constants;
const {Item} = Form;

export const ModalAddEdit = Form.create({name: 'modal_add_edit'})(
  class extends Component {
    constructor(props) {
      super(props);
      this.state = {
        ThanhVienHoiDong: []
      };
    }

    onOk = (e) => {
      e.preventDefault();
      this.props.form.validateFields((err, value) => {
        if (!err) {
          if (value.ThanhVienHoiDong.length > 0) {
            for (let i = 0; i < value.ThanhVienHoiDong.length; i++) {
              if (!value.ThanhVienHoiDong[i].CanBoID) {
                message.destroy();
                message.warning('Chưa chọn cán bộ thành viên');
                return;
              }
            }
            const {onCreate} = this.props;
            onCreate(value);
          }
          else {
            message.destroy();
            message.warning('Chưa chọn cán bộ thành viên');
          }
        }
      });
    };

    onCancel = () => {
      const {onCancel} = this.props;
      onCancel();
    };

    componentDidMount() {
      const {dataEdit, actions} = this.props;
      if (actions === 'edit') {
        const ThanhVienHoiDong = dataEdit.ThanhVienHoiDong;
        this.setState({ThanhVienHoiDong})
      } else {
        const ThanhVienHoiDong = [{
          CanBoID: undefined,
          VaiTro: ""
        }];
        this.setState({ThanhVienHoiDong})
      }
    }

    addThanhVien = () => {
      const {ThanhVienHoiDong} = this.state;
      ThanhVienHoiDong.push({
        CanBoID: undefined,
        VaiTro: ""
      });
      this.setState({ThanhVienHoiDong})
    };

    removeThanhVien = (index) => {
      const {ThanhVienHoiDong} = this.state;
      ThanhVienHoiDong.splice(index, 1);
      this.setState({ThanhVienHoiDong})
    };

    changeValue = (value, index, field) => {
      const {ThanhVienHoiDong} = this.state;
      ThanhVienHoiDong[index][field] = value;
      this.setState({ThanhVienHoiDong})
    };

    render() {
      const {visible, onCancel, form, loading, DanhSachCanBo, dataEdit, actions} = this.props;
      const {ThanhVienHoiDong} = this.state;
      const {getFieldDecorator} = form;
      const title = `${actions === 'add' ? "Thêm" : "Cập nhật"} thông tin hội đồng`;
      return (
        <Modal
          title={title}
          width={900}
          onCancel={this.onCancel}
          visible={visible}
          footer={[
            <Button key="back" onClick={onCancel}>Hủy</Button>,
            <Button key="submit" htmlType="submit" type="primary" form="form-hoi-dong"
                    onClick={this.onOk} loading={loading} disabled={loading}>Lưu</Button>,
          ]}
        >
          <Form id="form-hoi-dong" style={{color: 'black'}}>
            {getFieldDecorator('HoiDongID', {initialValue: actions === 'edit' ? dataEdit.HoiDongID : null})}
            <Item label="Tên hội đồng" {...ITEM_LAYOUT2}>
              {getFieldDecorator('TenHoiDong', {
                initialValue: actions === 'edit' ? dataEdit.TenHoiDong : "", rules: [{...REQUIRED}]
              })(
                <Input autoFocus/>)}
            </Item>
            <Row>
              <Col span={20}>Thành viên hội đồng</Col>
              <Col span={4} style={{textAlign: 'right'}}>
                <Button icon={'plus'} type={"primary"} onClick={this.addThanhVien} style={{width: 25, height: 25}}/>
              </Col>
            </Row>
            {getFieldDecorator('ThanhVienHoiDong', {initialValue: ThanhVienHoiDong})}
            {ThanhVienHoiDong.map((item, index) => (
              <Row>
                <Col {...COL_ITEM_LAYOUT_HALF2}>
                  <Item label={"Họ và tên"} {...ITEM_LAYOUT_HALF2}>
                    <Select showSearch value={ThanhVienHoiDong[index].CanBoID}
                            onChange={value => this.changeValue(value, index, 'CanBoID')} maxTagCount={20}>
                      {DanhSachCanBo.map(item => (
                        <Option value={item.CanBoID}>{item.TenCanBo}</Option>
                      ))}
                    </Select>
                  </Item>
                </Col>
                <Col {...COL_ITEM_LAYOUT_HALF2}>
                  <Row>
                    <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                      <Item label={"Vai trò"} {...ITEM_LAYOUT_HALF2}>
                        <Input value={ThanhVienHoiDong[index].VaiTro} style={{width: '80%'}}
                               onChange={value => this.changeValue(value.target.value, index, 'VaiTro')}/>
                        <div style={{float: 'right'}}>
                          <Button icon={'minus'} onClick={() => this.removeThanhVien(index)}
                                  style={{width: 25, height: 25}}/>
                        </div>
                      </Item>
                    </Col>
                  </Row>
                </Col>
              </Row>
            ))}
          </Form>
        </Modal>
      );
    }
  },
);