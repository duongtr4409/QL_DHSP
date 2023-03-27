import React, { Component } from "react";
import Constants, { MODAL_NORMAL, ITEM_LAYOUT3, REQUIRED, COL_ITEM_LAYOUT_HALF, ITEM_LAYOUT_HALF3, COL_COL_ITEM_LAYOUT_RIGHT } from "../../../settings/constants";
import { Modal, Form, Input, Switch, Button, Row, Col, Radio, message, Checkbox } from "antd";
import { ValidatorForm } from "react-form-validator-core";
// import TreeSelect from "../../../components/uielements/treeSelect";
// import Select, { Option } from "../../../components/uielements/select";
import api from "./config";
import { GoInput, GoSelect, withAPI } from "../../components/index";

const SelectWithApi = withAPI(GoSelect);
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
          Code: "",
          ParentId: null,
          Type: 1,
          Status: true,
          Children: [],
        },

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
      // this.setState({ confirmLoading: true });
      const { editNode, parentNode } = this.state;

      if (this.props.editNode) {
        // Nếu là sửa
        api.suaLinhVuc(editNode).then((res) => {
          if (!res || !res.data || res.data.Status !== 1) {
            message.error(`${res.data ? res.data.Message : "Lỗi hệ thống"}`);
          } else {
            message.success("Sửa thành công");
            this.props.onClose();
          }
        });
      } else {
        // Nếu là thêm mới 1 danh mục

        if (parentNode) {
          editNode.ParentId = parentNode.Id;
          editNode.Type = parentNode.Type;
        }

        api.themLinhVuc(editNode).then((res) => {
          if (!res || !res.data || res.data.Status !== 1) {
            message.error(`${res.data ? res.data.Message : "Lỗi hệ thống"}`);
          } else {
            message.success("Thêm mới thành công");
            this.props.onClose();
          }
        });
      }
    };

    render() {
      const { parentNode } = this.props;
      const { editNode } = this.state;
      return (
        <Modal
          title={`${this.props.editNode ? "Sửa" : "Thêm"} thông tin lĩnh vực`}
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
            {this.state.parentNode ? (
              <Item label="Lĩnh vực cha" {...ITEM_LAYOUT3}>
                <Input disabled value={this.state.parentNode.Name} />
              </Item>
            ) : null}
            {this.props.parentNode ? (
              ""
            ) : (
              <div className="mb-3">
                <Item label="Loại lĩnh vực" {...ITEM_LAYOUT3}>
                  <Radio.Group
                    onChange={(event) => {
                      const { editNode } = this.state;
                      editNode.Type = event.target.value;
                      this.setState({ editNode });
                    }}
                    value={editNode.Type}
                  >
                    <Radio value={1}>Nghiên cứu KHCN</Radio>

                    <Radio value={2}>Kinh tế - Xã hội</Radio>
                  </Radio.Group>
                </Item>
              </div>
            )}

            <div>
              <Item label="Mã lĩnh vực" {...ITEM_LAYOUT3}>
                <GoInput
                  onChange={(event) => {
                    const { editNode } = this.state;
                    editNode.Code = event.target.value;
                    this.setState({ editNode });
                  }}
                  value={editNode.Code}
                  validators={["required"]}
                  errorMessages={["Nội dung bắt buộc"]}
                ></GoInput>
              </Item>
              <Item label="Tên lĩnh vực" {...ITEM_LAYOUT3}>
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

            <Row>
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
