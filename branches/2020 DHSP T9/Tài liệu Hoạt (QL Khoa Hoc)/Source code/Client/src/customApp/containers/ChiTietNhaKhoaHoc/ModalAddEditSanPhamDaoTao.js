import React, {Component} from 'react';
import {Button, Form, Input, Modal, Row, Col, Upload, message, Icon, InputNumber} from 'antd';
import Constants, {LoaiThongTinNhaKhoaHoc, REQUIRED} from "../../../settings/constants";
import Select, {Option} from "../../../components/uielements/select4";
import {formatDataTreeSelect} from "../../../helpers/utility";
import TreeSelectEllipsis from "../../components/TreeSelectEllipsis";
import apiConfig from "../DataCoreAPI/config";

const {
  ITEM_LAYOUT2, ITEM_LAYOUT_HALF2,
  COL_ITEM_LAYOUT_HALF2,
  COL_COL_ITEM_LAYOUT_RIGHT2,
} = Constants;
const {Item} = Form;

export const ModalAddEditSanPhamDaoTao = Form.create({name: 'modal_add_edit'})(
  class extends Component {
    constructor(props) {
      super(props);
      this.state = {
        ListTacGia: [],
        DanhSachNhiemVu: []
      }
    }

    onOk = (e) => {
      e.preventDefault();
      this.props.form.validateFields((err, value) => {
        if (!err) {
          const {onCreate} = this.props;
          // if (value.TacGia.length > 0) {
          //   value.ListTacGia = value.TacGia.map(item => {
          //     const arr = item.split("_");
          //     return {
          //       CanBoID: arr[0],
          //       CoQuanID: arr[1]
          //     }
          //   })
          // }
          // delete value.TacGia;
          onCreate(value);
        }
      });
    };

    componentDidMount() {
      const {dataEdit} = this.props;
      if (dataEdit.ListTacGia) {
        const ListTacGia = dataEdit.ListTacGia.map(item => `${item.CanBoID}_${item.CoQuanID}`);
        this.setState({ListTacGia})
      }
      if (dataEdit.LoaiNhiemVu) {
        this.getDanhSachNhiemVu(dataEdit.LoaiNhiemVu, false);
      }
    }

    onCancel = () => {
      const {onCancel} = this.props;
      onCancel();
    };

    getDanhSachNhiemVu = (LoaiNhiemVuID, clear = true) => {
      if (clear || !LoaiNhiemVuID) {
        this.props.form.setFieldsValue({NhiemVu: undefined});
      }
      if (LoaiNhiemVuID) {
        apiConfig.DanhSachNhiemVuQuyDoi({categoryId: LoaiNhiemVuID}).then(response => {
          if (response.data.Status > 0) {
            const DanhSachNhiemVu = formatDataTreeSelect(response.data.Data, false);
            this.setState({DanhSachNhiemVu});
          }
        })
      } else {
        this.setState({DanhSachNhiemVu: []})
      }
    };

    render() {
      const {visible, onCancel, form, dataEdit, loading, actions, DanhSachCanBo, DanhSachDeTai, DanhSachLoaiNhiemVu} = this.props;
      const {getFieldDecorator} = form;
      const {DanhSachNhiemVu} = this.state;
      let titleModal = "Thêm thông tin kết quả đào tạo";
      if (actions === 'edit') {
        titleModal = "Cập nhật thông tin kết quả đào tạo";
      }

      return (
        <Modal
          title={titleModal}
          width={900}
          onCancel={this.onCancel}
          visible={visible}
          footer={[
            <Button key="back" onClick={onCancel}>Hủy</Button>,
            <Button key="submit" htmlType="submit" type="primary" form="formspdaotao"
                    onClick={this.onOk} loading={loading} disabled={loading}>Lưu</Button>,
          ]}
        >
          <Form id="formspdaotao">
            {getFieldDecorator('CTNhaKhoaHocID', {initialValue: dataEdit.CTNhaKhoaHocID ? dataEdit.CTNhaKhoaHocID : null})}
            {getFieldDecorator('CanBoID', {initialValue: this.props.CanBoID})}
            {getFieldDecorator('CoQuanID', {initialValue: this.props.CoQuanID})}
            {getFieldDecorator('LoaiThongTin', {initialValue: LoaiThongTinNhaKhoaHoc.SanPhamDaoTao})}
            <Item label={"Đề tài"} {...ITEM_LAYOUT2}>
              {getFieldDecorator('DeTai', {
                initialValue: dataEdit.DeTai ? dataEdit.DeTai : undefined
              })(<Select autoFocus showSearch>
                {DanhSachDeTai.map(item => (
                  <Option value={item.DeTaiID}>{item.TenDeTai}</Option>
                ))}
              </Select>)}
            </Item>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item label={"Loại nhiệm vụ"} {...ITEM_LAYOUT_HALF2}>
                  {getFieldDecorator('LoaiNhiemVu', {
                    initialValue: dataEdit.LoaiNhiemVu ? dataEdit.LoaiNhiemVu : undefined
                  })(
                    <TreeSelectEllipsis
                      showSearch
                      data={DanhSachLoaiNhiemVu}
                      defaultValue={undefined}
                      dropdownStyle={{maxHeight: 400, overflowX: 'hidden', maxWidth: 500}}
                      placeholder=""
                      allowClear
                      notFoundContent={"Không có dữ liệu"}
                      treeNodeFilterProp={'label'}
                      onChange={this.getDanhSachNhiemVu}
                    />
                  )}
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                    <Item label={"Nhiệm vụ"} {...ITEM_LAYOUT_HALF2}>
                      {getFieldDecorator('NhiemVu', {
                        initialValue: dataEdit.NhiemVu ? dataEdit.NhiemVu : undefined
                      })(
                        <TreeSelectEllipsis
                          showSearch
                          data={DanhSachNhiemVu}
                          defaultValue={undefined}
                          dropdownStyle={{maxHeight: 400, overflowX: 'hidden', maxWidth: 500}}
                          placeholder=""
                          allowClear
                          notFoundContent={"Không có dữ liệu"}
                          treeNodeFilterProp={'label'}
                        />
                      )}
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Item label={"Họ tên"} {...ITEM_LAYOUT2}>
              {getFieldDecorator('TenHocVien', {
                initialValue: dataEdit.TenHocVien ? dataEdit.TenHocVien : "", rules: [{...REQUIRED}]
              })(<Input maxLength={500}/>)}
            </Item>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item label={"Bậc học"} {...ITEM_LAYOUT_HALF2}>
                  {getFieldDecorator('LoaiDaoTao', {
                    initialValue: dataEdit.LoaiDaoTao ? dataEdit.LoaiDaoTao : undefined
                  })(
                    <Select showSearch allowClear>
                      <Option value={1}>Cử nhân</Option>
                      <Option value={2}>Thạc sỹ</Option>
                      <Option value={3}>Tiến sỹ</Option>
                      {/*<Option value={4}>Nghiên cứu sinh</Option>*/}
                    </Select>
                  )}
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                    <Item label={"Trách nhiệm"} {...ITEM_LAYOUT_HALF2}>
                      {getFieldDecorator('CapHoiThao', {
                        initialValue: dataEdit.CapHoiThao ? dataEdit.CapHoiThao : undefined
                      })(
                        <Select showSearch allowClear>
                          <Option value={1}>Chính</Option>
                          <Option value={2}>Phụ</Option>
                        </Select>
                      )}
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Item label={"Tên khóa luận/ luận văn/ luận án"} {...ITEM_LAYOUT2}>
              {getFieldDecorator('TenLuanVan', {
                initialValue: dataEdit.TenLuanVan ? dataEdit.TenLuanVan : "", rules: [{...REQUIRED}]
              })(<Input maxLength={500}/>)}
            </Item>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item label={"Thời gian hướng dẫn"} {...ITEM_LAYOUT_HALF2}>
                  {getFieldDecorator('KhoangThoiGian', {
                    initialValue: dataEdit.KhoangThoiGian ? dataEdit.KhoangThoiGian : ""
                  })(<Input maxLength={500}/>)}
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                    <Item label={"Cơ sở đào tạo"} {...ITEM_LAYOUT_HALF2}>
                      {getFieldDecorator('CoSoDaoTao', {
                        initialValue: dataEdit.CoSoDaoTao ? dataEdit.CoSoDaoTao : ""
                      })(<Input maxLength={500}/>)}
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item label={"Năm bảo vệ"} {...ITEM_LAYOUT_HALF2}>
                  {getFieldDecorator('NamBaoVe', {
                    initialValue: dataEdit.NamBaoVe ? dataEdit.NamBaoVe : "", rules: [{...REQUIRED}]
                  })(<InputNumber/>)}
                </Item>
              </Col>
            </Row>
          </Form>
        </Modal>
      );
    }
  },
);