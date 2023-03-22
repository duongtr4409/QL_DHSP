import React, { Component } from "react";
import { Button, Col, Form, Icon, Input, message, Modal, Row, InputNumber } from "antd";
import Constants, { LoaiThongTinNhaKhoaHoc, REQUIRED } from "../../../../../settings/constants";
import Select, { Option } from "../../../../../components/uielements/select4";
import { checkValidFileName, formatDataTreeSelect } from "../../../../../helpers/utility";
import TreeSelectEllipsis from "../../../../components/TreeSelectEllipsis";
import apiConfig from "../../../DataCoreAPI/config";
import Datepicker from "../../../../../components/uielements/datePickerFormat";
import moment from "moment";

const { ITEM_LAYOUT2, ITEM_LAYOUT_HALF2, COL_ITEM_LAYOUT_HALF2, COL_COL_ITEM_LAYOUT_RIGHT2 } = Constants;
const { Item } = Form;

export const ModalAddEditSachChuyenKhao = Form.create({ name: "modal_add_edit" })(
  class extends Component {
    constructor(props) {
      super(props);
      this.state = {
        ListTacGia: [],
        ChuBien: undefined,
        DanhSachNhiemVu: [],
      };
    }

    onOk = (e) => {
      e.preventDefault();
      this.props.form.validateFields((err, value) => {
        if (!err) {
          const { onCreate } = this.props;
          if (value.TacGia.length > 0) {
            value.ListTacGia = value.TacGia.map((item) => {
              const arr = item.split("_");
              return {
                CanBoID: arr[0],
                CoQuanID: arr[1],
              };
            });
          }
          delete value.TacGia;
          if (value.ChuBien) {
            const arr = value.ChuBien.split("_");
            value.ChuBienID = arr[0];
            value.CoQuanChuBienID = arr[1];
          }
          delete value.ChuBien;
          onCreate(value);
        }
      });
    };

    componentDidMount() {
      const { dataEdit } = this.props;
      if (dataEdit.ListTacGia) {
        const ListTacGia = dataEdit.ListTacGia.map((item) => `${item.CanBoID}_${item.CoQuanID}`);
        this.setState({ ListTacGia });
      }
      if (dataEdit.ChuBienID) {
        const ChuBien = `${dataEdit.ChuBienID}_${dataEdit.CoQuanChuBienID}`;
        this.setState({ ChuBien });
      }
      if (dataEdit.LoaiNhiemVu) {
        this.getDanhSachNhiemVu(dataEdit.LoaiNhiemVu, false);
      }
    }

    onCancel = () => {
      const { onCancel } = this.props;
      onCancel();
    };

    getDanhSachNhiemVu = (LoaiNhiemVuID, clear = true) => {
      if (clear || !LoaiNhiemVuID) {
        this.props.form.setFieldsValue({ NhiemVu: undefined });
      }
      if (LoaiNhiemVuID) {
        apiConfig.DanhSachNhiemVuQuyDoi({ categoryId: LoaiNhiemVuID }).then((response) => {
          if (response.data.Status > 0) {
            const DanhSachNhiemVu = formatDataTreeSelect(response.data.Data, false);
            this.setState({ DanhSachNhiemVu });
          }
        });
      } else {
        this.setState({ DanhSachNhiemVu: [] });
      }
    };
    setConfirmLoading = (confirmLoading) => {
      this.setState({ confirmLoading });
    };

    render() {
      const { visible, onCancel, form, dataEdit, loading, actions, DanhSachCanBo, DanhSachDeTai, DanhSachLoaiNhiemVu } = this.props;
      const { DanhSachNhiemVu, confirmLoading } = this.state;
      const { getFieldDecorator } = form;
      const { ListTacGia, ChuBien } = this.state;
      let titleModal = "Thêm thông tin sách";
      if (actions === "edit") {
        titleModal = "Cập nhật thông tin sách";
      }

      return (
        <Modal
          title={titleModal}
          width={800}
          onCancel={this.onCancel}
          visible={true}
          confirmLoading={confirmLoading}
          footer={[
            <Button key="back" onClick={onCancel}>
              Hủy
            </Button>,
            <Button key="submit" htmlType="submit" type="primary" form="formsach" onClick={this.onOk} loading={confirmLoading} disabled={confirmLoading}>
              Lưu
            </Button>,
          ]}
        >
          <Form id="formsach">
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item label={"Loại nhiệm vụ"} {...ITEM_LAYOUT_HALF2}>
                  {getFieldDecorator("LoaiNhiemVu", {
                    initialValue: dataEdit.LoaiNhiemVu ? dataEdit.LoaiNhiemVu : undefined,
                  })(
                    <TreeSelectEllipsis
                      showSearch
                      data={DanhSachLoaiNhiemVu}
                      defaultValue={undefined}
                      dropdownStyle={{ maxHeight: 400, overflowX: "hidden", maxWidth: 500 }}
                      placeholder=""
                      allowClear
                      notFoundContent={"Không có dữ liệu"}
                      treeNodeFilterProp={"label"}
                      onChange={this.getDanhSachNhiemVu}
                    />
                  )}
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                    <Item label={"Nhiệm vụ"} {...ITEM_LAYOUT_HALF2}>
                      {getFieldDecorator("NhiemVu", {
                        initialValue: dataEdit.NhiemVu ? dataEdit.NhiemVu : undefined,
                      })(
                        <TreeSelectEllipsis
                          showSearch
                          data={DanhSachNhiemVu}
                          defaultValue={undefined}
                          dropdownStyle={{ maxHeight: 400, overflowX: "hidden", maxWidth: 500 }}
                          placeholder=""
                          allowClear
                          notFoundContent={"Không có dữ liệu"}
                          treeNodeFilterProp={"label"}
                        />
                      )}
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item label={"Tên sách"} {...ITEM_LAYOUT_HALF2}>
                  {getFieldDecorator("TenTapChiSachHoiThao", {
                    initialValue: dataEdit.TenTapChiSachHoiThao ? dataEdit.TenTapChiSachHoiThao : "",
                    rules: [{ ...REQUIRED }],
                  })(<Input maxLength={500} />)}
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                    <Item label={"Chủ biên"} {...ITEM_LAYOUT_HALF2}>
                      {getFieldDecorator("ChuBien", {
                        initialValue: ChuBien,
                      })(
                        <Select showSearch allowClear>
                          {DanhSachCanBo.map((item) => (
                            <Option value={`${item.CanBoID}_${item.CoQuanID}`}>{item.TenCanBo}</Option>
                          ))}
                        </Select>
                      )}
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Item label={"Tác giả"} {...ITEM_LAYOUT2}>
              {getFieldDecorator("TacGia", {
                initialValue: ListTacGia,
                rules: [{ ...REQUIRED }],
              })(
                <Select showSearch allowClear mode="multiple">
                  {DanhSachCanBo.map((item) => (
                    <Option value={`${item.CanBoID}_${item.CoQuanID}`}>{item.TenCanBo}</Option>
                  ))}
                </Select>
              )}
            </Item>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item label={"Năm xuất bản"} {...ITEM_LAYOUT_HALF2}>
                  {getFieldDecorator("NamXuatBan", {
                    initialValue: dataEdit.NamXuatBan ? dataEdit.NamXuatBan : "",
                    rules: [{ ...REQUIRED }],
                  })(<InputNumber />)}
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                    <Item label={"Nhà xuất bản"} {...ITEM_LAYOUT_HALF2}>
                      {getFieldDecorator("NhaXuatBan", {
                        initialValue: dataEdit.NhaXuatBan ? dataEdit.NhaXuatBan : "",
                      })(<Input />)}
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Item label={"Chỉ số xuất bản (ISSN)"} {...ITEM_LAYOUT2}>
              {getFieldDecorator("ISSN", {
                initialValue: dataEdit.ISSN ? dataEdit.ISSN : "",
              })(<Input />)}
            </Item>
          </Form>
        </Modal>
      );
    }
  }
);
