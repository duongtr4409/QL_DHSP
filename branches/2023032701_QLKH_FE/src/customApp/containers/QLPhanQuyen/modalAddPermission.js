import React, {Component} from "react";
import Constants, {MODAL_NORMAL, ITEM_LAYOUT3, REQUIRED} from "../../../settings/constants";
import {Modal, Form, Input, Button, Checkbox} from "antd";
import Select, {Option, OptGroup} from "../../../components/uielements/select";
import {EmptyTable} from "../../../components/utility/boxTable";
import {getOptionSidebarGroup, specialPermission} from "../../../helpers/utility";

const {Item} = Form;
const optionsSidebar = getOptionSidebarGroup();

const ModalAddPermission = Form.create({name: "modal_add_permission"})(
  // eslint-disable-next-line
  class extends Component {
    constructor(props) {
      super(props);
      this.state = {
        allRight: false,
        NhomNguoiDungID: 0,
        DanhSachChucNang: [],
        DanhSachChucNangThem: [],
        DS: [],
        options: [],
        defaultValue: [],
        DanhSachNhomChucNang: [],
        permissions: [],
      };
    }

    componentDidMount() {
      const {NhomNguoiDungID, DanhSachChucNang} = this.props.dataModalAddPermission;
      let DanhSachNhomChucNang = [];

      optionsSidebar.forEach((group) => {
        if (group.children) {
          let childrenKeys = group.children.map((children) => children.key);
          DanhSachChucNang.some((option) => {
            //danh sach chuc nang cha
            // console.log(childrenKeys, option.MaChucNang);
            if (childrenKeys.indexOf(option.MaChucNang) >= 0) {
              DanhSachNhomChucNang.push({
                key: group.key,
                label: group.label,
                childrenKeys,
              });
              return true;
            }
            return false;
          });
        } else {
          DanhSachChucNang.some((option) => {
            //danh sach chuc nang cha
            if (group.key === option.MaChucNang) {
              DanhSachNhomChucNang.push({
                key: group.key,
                label: group.label,
                isToggle: group.isToggle,
              });
              return true;
            }
            return false;
          });
        }
      });
      //get DS
      let DS = [];
      DanhSachNhomChucNang.forEach((groupValue) => {
        let parent = {
          id: groupValue.key,
          key: groupValue.key,
          label: groupValue.label,
          disabled: !!groupValue.children,
          isParent: true,
        };
        if (groupValue.childrenKeys) {
          parent.children = [];
          let children = [];
          DanhSachChucNang.forEach((value) => {
            if (groupValue.childrenKeys.indexOf(value.MaChucNang) >= 0) {
              children.push({
                id: value.ChucNangID,
                key: value.MaChucNang,
                label: value.TenChucNang,
                disabled: value.disabled,
                children: [],
                isParent: false,
              });
              if (value.disabled === false) parent.disabled = false;
            }
          });
          parent.children = children;
          DS.push(parent);
          children.forEach((childrenItem) => {
            DS.push(childrenItem);
          });
        } else
          DanhSachChucNang.forEach((value) => {
            if (parent.key === value.MaChucNang) {
              parent.id = value.ChucNangID;
              parent.disabled = value.disabled;
              parent.isToggle = value.isToggle;
              DS.push(parent);
            }
          });
      });
      //--
      // console.log(DS, 'ds');
      this.setState({
        allRight: NhomNguoiDungID,
        NhomNguoiDungID,
        DanhSachChucNang,
        DanhSachNhomChucNang,
        DS,
      });
    }

    onOk = (e) => {
      e.preventDefault();
      this.props.form.validateFields((err, value) => {
        if (!err) {
          const data = this.state.permissions;
          const {onCreate} = this.props;
          onCreate(data);
        }
      });
    };

    onChangePermission = (checkedValues, ChucNangID) => {
      if (checkedValues.length) {
        let permissions = [...this.state.permissions];
        permissions.some((item, index) => {
          if (item.ChucNangID === ChucNangID) {
            permissions[index] = {
              NhomNguoiDungID: this.state.NhomNguoiDungID,
              ChucNangID: ChucNangID,
              Xem: checkedValues.indexOf("Xem") >= 0 ? 1 : 0,
              Them: checkedValues.indexOf("Them") >= 0 ? 1 : 0,
              Sua: checkedValues.indexOf("Sua") >= 0 ? 1 : 0,
              Xoa: checkedValues.indexOf("Xoa") >= 0 ? 1 : 0,
            };
            return true;
          }
          return false;
        });
        this.setState({permissions});
      } else {
        this.deleteOption(ChucNangID);
      }
    };

    renderOptions = (DanhSachChucNang) => {
      let optionsComponent = "";
      if (DanhSachChucNang && DanhSachChucNang.length) {
        optionsComponent = DanhSachChucNang.map((item, index) => {
          let options = [],
            defaultValue = [];
          const ChucNangID = item.ChucNangID;
          //get parent item from all list chuc nang
          let parentItem = null;
          const DanhSachChucNangCha = this.state.DanhSachChucNang;
          DanhSachChucNangCha.some((pItem) => {
            if (pItem.ChucNangID === ChucNangID) {
              parentItem = {...pItem};
              return true;
            }
            return false;
          });
          if (parentItem) {
            options = [
              {label: "Xem", value: "Xem", disabled: parentItem.Xem === 0},
              {label: "Thêm", value: "Them", disabled: parentItem.Them === 0},
              {label: "Sửa", value: "Sua", disabled: parentItem.Sua === 0},
              {label: "Xóa", value: "Xoa", disabled: parentItem.Xoa === 0},
            ];
            if (item.Xem) defaultValue.push("Xem");
            if (item.Them) defaultValue.push("Them");
            if (item.Sua) defaultValue.push("Sua");
            if (item.Xoa) defaultValue.push("Xoa");

            return (
              <div key={item.ChucNangID} className="content_row">
                <div className="tenchucnang" style={{display: "inline-block", width: 184}}>
                  <b>{item.TenChucNang}</b>
                </div>
                <div className="chonxoaquyen" style={{display: "inline-block"}}>
                  {specialPermission.findIndex((d) => d.key === item.MaChucNang) !== -1 ? (
                    <Checkbox.Group defaultValue={defaultValue}
                                    options={[{label: "", value: "Xem", disabled: parentItem.Xem === 0}]}
                                    onChange={(checkedValue) => this.onChangePermission(checkedValue, item.ChucNangID)}/>
                  ) : (
                    <Checkbox.Group options={options} defaultValue={defaultValue}
                                    onChange={(checkedValue) => this.onChangePermission(checkedValue, item.ChucNangID)}/>
                  )}

                  <button style={{border: "none", background: "none", outline: "none", cursor: "pointer"}}
                          onClick={() => this.deleteOption(item.ChucNangID)}>
                    ✖
                  </button>
                </div>
              </div>
            );
          }
        });
      }
      return optionsComponent;
    };

    deleteOption = (ChucNangID) => {
      let DanhSachChucNangThem = [];
      let DanhSachChucNangThemID = [];
      let permissions = [];
      this.state.DanhSachChucNangThem.forEach((item) => {
        if (item.ChucNangID !== ChucNangID) {
          DanhSachChucNangThem.push(item);
          DanhSachChucNangThemID.push(item.ChucNangID);
        }
      });
      this.state.permissions.forEach((item) => {
        if (item.ChucNangID !== ChucNangID) {
          permissions.push(item);
        }
      });
      this.setState({DanhSachChucNangThem, permissions}, () => {
        this.props.form.setFieldsValue({DanhSachChucNangThemID});
      });
    };

    onChange = (MangChucNangID) => {
      //result = MangChucNangID = [1, 'HeThong', 2, 3] -> [1,  1,2,3,  2, 3] \ HeThong = [1, 2, 3]
      let result = [];
      MangChucNangID.forEach((id, index) => {
        if (isNaN(id)) {
          this.state.DS.forEach((dsItem) => {
            if (dsItem.id === id) {
              if (dsItem.children) {
                dsItem.children.forEach((childrenItem) => {
                  if (childrenItem.disabled === false) {
                    result.push(childrenItem.id);
                  }
                });
              } else {
                if (dsItem.disabled === false) {
                  result.push(dsItem.id);
                }
              }
            }
          });
        } else {
          result.push(id);
        }
      });
      //sap xep result, xoa phan tu trung nhau
      let MangID = [];
      if (result.length) {
        this.state.DS.forEach((dsItem) => {
          if (!isNaN(dsItem.id) && result.indexOf(dsItem.id) >= 0) MangID.push(dsItem.id);
        });
      }
      this.addOption(MangID);
    };

    addOption = (MangChucNangID) => {
      let DanhSachChucNangThem = [];
      let permissions = MangChucNangID && MangChucNangID.length ? [...this.state.permissions] : [];
      this.state.DanhSachChucNang.forEach((parentItem) => {
        if (MangChucNangID.indexOf(parentItem.ChucNangID) >= 0) {
          DanhSachChucNangThem.push(parentItem);
          // ---
          let exist = false;
          permissions.forEach((itemPermission, indexPermission) => {
            //check exist neu chua thi them
            if (parentItem.ChucNangID === itemPermission.ChucNangID) exist = true;
            //check xem quyen co bi xoa di khong
            if (MangChucNangID.indexOf(itemPermission.ChucNangID) < 0) permissions.splice(indexPermission, 1);
          });
          if (!exist) {
            let permission = {
              NhomNguoiDungID: this.state.NhomNguoiDungID,
              ChucNangID: parentItem.ChucNangID,
              Xem: parentItem.Xem ? 1 : 0,
              Them: parentItem.Them ? 1 : 0,
              Sua: parentItem.Sua ? 1 : 0,
              Xoa: parentItem.Xoa ? 1 : 0,
            };
            permissions.push(permission);
          }
        }
      });
      this.setState({DanhSachChucNangThem, permissions}, () => {
        this.props.form.setFieldsValue({DanhSachChucNangThemID: MangChucNangID});
      });
    };

    render() {
      if (!this.state.allRight) return null;
      const {confirmLoading, visible, onCancel, form} = this.props;
      const {getFieldDecorator} = form;
      return (
        <Modal
          title="Thêm chức năng cho nhóm"
          width={MODAL_NORMAL}
          visible={visible}
          onCancel={onCancel}
          footer={[
            <Button key="back" onClick={onCancel}>
              Hủy
            </Button>,
            <Button key="submit" htmlType="submit" type="primary" form="myForm" loading={confirmLoading}
                    onClick={this.onOk}>
              Lưu
            </Button>,
          ]}
        >
          <Form id="myForm" layout="horizontal">
            <Item label="Chọn chức năng" {...ITEM_LAYOUT3}>
              {getFieldDecorator("DanhSachChucNangThemID", {
                rules: [{...REQUIRED}],
              })(
                <Select showSearch placeholder="Chọn chức năng" onChange={this.onChange}
                        defaultActiveFirstOption={false} allowClear mode="multiple" style={{marginTop: 4}}
                        className="scroll-select-selection--multiple">
                  {this.state.DS.map((item) => {
                    if (item.isParent) {
                      return (
                        <Option key={item.key} value={item.id} disabled={item.disabled} style={{fontWeight: "bold"}}>
                          {item.label}
                        </Option>
                      );
                    } else {
                      return (
                        <Option key={item.key} value={item.id} disabled={item.disabled} style={{paddingLeft: 20}}>
                          {item.label}
                        </Option>
                      );
                    }
                  })}
                </Select>
              )}
            </Item>
            {this.state.DanhSachChucNangThem && this.state.DanhSachChucNangThem.length ? this.renderOptions(this.state.DanhSachChucNangThem) : null}
          </Form>
        </Modal>
      );
    }
  }
);
export {ModalAddPermission};
