import React, { Component } from "react";
import Constants, { MODAL_NORMAL, ITEM_LAYOUT3, REQUIRED, COL_ITEM_LAYOUT_HALF, ITEM_LAYOUT_HALF3, COL_COL_ITEM_LAYOUT_RIGHT } from "../../../settings/constants";
import { Modal, Form, Input, Switch, Button, Row, Col, Radio, message, Checkbox } from "antd";
import { ValidatorForm } from "react-form-validator-core";
// import TreeSelect from "../../../components/uielements/treeSelect";
// import Select, { Option } from "../../../components/uielements/select";
import api from "./config";
import { GoInput, GoSelect, withAPI, SelectWithApi, TreeSelectWithApi, GoDatePicker } from "../../components/index";

const { Item } = Form;

const ModalAdd = Form.create({ name: "modal_add_DG" })(
  // eslint-disable-next-line
  class extends Component {
    constructor(props) {
      super(props);
      this.submitBtn = React.createRef();
      this.state = {
        parentNode: null,
        editNode: {
          Name: "",
          ParentId: null,

          Status: true,
          Children: [],
        },
        isAddNew: true,
        confirmLoading: false,
        selectedValues: [],
        selectedOptions: [],
      };
    }

    componentDidMount() {
      const { parentNode, editNode } = this.props;

      this.state.parentNode = parentNode;
      if (editNode) {
        this.setState({ editNode });
      } else {
        this.setState({ parentNode });
      }
    }
    handleSubmit = () => {
      this.setState({ confirmLoading: true });
      const { editNode, parentNode } = this.state;
      if (parentNode && parentNode.Type) {
        editNode.Type = parentNode.Type;
      }
      if (this.props.editNode) {
        // Nếu là sửa
        api.suaCapDeTai(editNode).then((res) => {
          if (!res || !res.data || res.data.Status !== 1) {
            message.error(`${res.data ? res.data.Message : "Lỗi hệ thống"}`);
            this.setState({ confirmLoading: false });
          } else {
            message.success("Sửa thành công");
            this.props.onClose();
            this.setState({ confirmLoading: false });
          }
        });
      } else {
        if (this.state.isAddNew) {
          // Nếu là thêm mới 1 danh mục

          if (parentNode) {
            editNode.ParentId = parentNode.Id;
          }

          api.themCapDeTai(editNode).then((res) => {
            if (!res || !res.data || res.data.Status !== 1) {
              message.error(`${res.data ? res.data.Message : "Lỗi hệ thống"}`);
              this.setState({ confirmLoading: false });
            } else {
              message.success("Thêm mới thành công");
              this.props.onClose();
              this.setState({ confirmLoading: false });
            }
          });
        } else {
          const { selectedOptions } = this.state;
          let danhSachNhieuCapDeTai = [];
          selectedOptions.forEach((item) => {
            const { Name, Id } = item;
            danhSachNhieuCapDeTai.push({
              Name,
              ParentId: parentNode.Id,
              MappingId: Id,
              Status: editNode.Status,
            });
          });
          // console.log(danhSachNhieuCapDeTai);
          api.themNhieuCapDeTai({ items: danhSachNhieuCapDeTai }).then((res) => {
            if (!res || !res.data || res.data.Status !== 1) {
              message.error(`${res && res.data ? res.data.Message : "Lỗi hệ thống"}`);
            } else {
              message.success("Thêm mới thành công");
              this.props.onClose();
              this.setState({ confirmLoading: false });
            }
          });
        }
      }
    };

    render() {
      const { parentNode } = this.props;
      const { editNode } = this.state;
      return (
        <Modal
          title={`${this.props.editNode ? "Sửa" : "Thêm"} thông tin cấp đề tài`}
          width={550}
          visible={true}
          onCancel={this.props.onClose}
          confirmLoading={this.state.confirmLoading}
          footer={[
            <Button key="back" onClick={this.props.onClose}>
              Hủy
            </Button>,
            <Button
              type="primary"
              loading={this.state.confirmLoading}
              onClick={() => {
                this.submitBtn.current.click();
              }}
            >
              Lưu
            </Button>,
          ]}
        >
          <ValidatorForm ref="form" onSubmit={this.handleSubmit}>
            {!this.props.editNode && parentNode ? (
              <Item label="Phân loại" {...ITEM_LAYOUT3}>
                <div className="mb-3">
                  <Radio.Group
                    onChange={(e) => {
                      this.setState({ isAddNew: e.target.value });
                    }}
                    value={this.state.isAddNew}
                  >
                    <Radio value={true}>Thêm mới</Radio>

                    <Radio value={false}>Chọn từ danh mục hệ thống</Radio>
                  </Radio.Group>
                </div>
              </Item>
            ) : (
              ""
            )}
            {parentNode && parentNode.Type !== 0 ? (
              ""
            ) : (
              <Item label="Thuộc loại" {...ITEM_LAYOUT3}>
                <div className="mb-3">
                  <Radio.Group
                    onChange={(e) => {
                      editNode.Type = e.target.value;
                      this.setState({ editNode });
                    }}
                    value={this.state.editNode.Type}
                  >
                    <Radio value={1}>Cấp trường</Radio>
                    <Radio value={2}>Cấp bộ</Radio>
                    <Radio value={3}>Cấp nhà nước</Radio>
                  </Radio.Group>
                </div>
              </Item>
            )}

            {this.state.isAddNew ? (
              <div>
                <Item label="Tên cấp đề tài" {...ITEM_LAYOUT3}>
                  <GoInput
                    onChange={(event) => {
                      const { editNode } = this.state;
                      editNode.Name = event.target.value;
                      this.setState({ editNode });
                    }}
                    value={editNode.Name}
                    validators={["required"]}
                    errorMessages={["Nội dung bắt buộc"]}
                  ></GoInput>
                </Item>
              </div>
            ) : (
              <Item label="Chọn cấp đề tài" {...ITEM_LAYOUT3}>
                <SelectWithApi
                  optionLabelProp="label"
                  placeholder="Chọn cấp đề tài"
                  apiConfig={{
                    api: api.danhSachCapDeTaiTuCore,
                    valueField: "Id",
                    nameField: "Name",
                  }}
                  mode="multiple"
                  returnFullItem
                  filter={{ parentId: "34" }}
                  onChange={(values, options) => {
                    this.setState({ selectedValues: values, selectedOptions: options });
                  }}
                  value={this.state.selectedValues}
                  validators={["required"]}
                  errorMessages={["Nội dung bắt buộc"]}
                ></SelectWithApi>
              </Item>
            )}
            {this.state.parentNode ? (
              <Item label="Cấp đề tài cha" {...ITEM_LAYOUT3}>
                <Input disabled value={this.state.parentNode.Name} />
              </Item>
            ) : null}
            <Row>
              <Col>
                <Item label="Đang sử dụng" {...ITEM_LAYOUT3}>
                  <Checkbox
                    checked={editNode.Status}
                    onChange={(event) => {
                      const { editNode } = this.state;
                      editNode.Status = event.target.checked;
                      this.setState({ editNode });
                    }}
                  ></Checkbox>
                </Item>
              </Col>
            </Row>

            <button className="d-none" ref={this.submitBtn} type="submit">
              submit
            </button>
          </ValidatorForm>
        </Modal>
      );
    }
  }
);
export { ModalAdd };
