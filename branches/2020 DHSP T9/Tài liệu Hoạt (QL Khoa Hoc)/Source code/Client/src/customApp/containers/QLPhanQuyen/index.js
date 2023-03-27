import React, { Component } from "react";
import { connect } from "react-redux";
import queryString from "query-string";
import actions from "../../redux/QLPhanQuyen/actions";
import api from "./config";
import Constants from "../../../settings/constants";
import lodash from "lodash";

import LayoutWrapper from "../../../components/utility/layoutWrapper";
import PageHeader from "../../../components/utility/pageHeader";
import PageAction from "../../../components/utility/pageAction";
import Box from "../../../components/utility/box";
import BoxFilter from "../../../components/utility/boxFilter";
import BoxTable, { EmptyTable } from "../../../components/utility/boxTable";
import { BoxTableDiv } from "./boxTableDiv.style";

import { ModalAddGroup } from "./modalAddGroup";
import { ModalEditGroup } from "./modalEditGroup";
import { ModalAddUser } from "./modalAddUser";
import { ModalAddPermission } from "./modalAddPermission";
import { BoxConfig } from "./boxConfig.style.js";
import { Modal, message, Input, Row, Col, Icon, Checkbox } from "antd";
import Button from "../../../components/uielements/button";
import Select, { Option } from "../../../components/uielements/select";
import TreeSelect from "../../../components/uielements/treeSelect";
import { changeUrlFilter, getDefaultPageSize, getFilterData, getOptionSidebarGroup, specialPermission } from "../../../helpers/utility";

const { confirm } = Modal;
let modal = null;

const optionsSidebar = getOptionSidebarGroup();

class QLPhanQuyen extends Component {
  constructor(props) {
    super(props);
    document.title = "Phân quyền";
    const filterData = queryString.parse(this.props.location.search);
    this.state = {
      filterData: filterData,
      selectedRowKeys: [],
      confirmLoading: false,
      modalKey: 0,
      visibleModalAddGroup: false,
      dataModalAddGroup: {
        DanhSachCoQuan: this.props.DanhSachCoQuan,
        DanhSachCoQuanID: [],
      },
      visibleModalEditGroup: false,
      dataModalEditGroup: {
        DanhSachCoQuan: this.props.DanhSachCoQuan,
        Data: null,
      },
      allList: [],
      visibleConfigGroup: false,
      configKey: 0,
      configData: null,
      dataModalAddUser: {
        NhomNguoiDungID: 0,
        DanhSachNguoiDung: [],
      },
      visibleModalAddUser: false,
      permissionsChanged: [],
      dataModalAddPermission: {
        NhomNguoiDungID: 0,
        DanhSachChucNang: [],
      },
      visibleModalAddPermission: false,
    };
  }

  //Get initData---------------------------------------------
  componentDidMount = () => {
    this.props.getInitData(this.state.filterData);
  };
  //filter --------------------------------------------------
  onFilter = (value, property) => {
    //get filter data
    let oldFilterData = this.state.filterData;
    if(typeof value == 'string')
    {
      value = value.trim();
    }
    let onFilter = { value, property };
    let filterData = getFilterData(oldFilterData, onFilter, null);
    //get filter data
    this.setState(
      {
        filterData,
        selectedRowKeys: [],
      },
      () => {
        changeUrlFilter(this.state.filterData); //change url
        this.props.getList(this.state.filterData); //get list
        this.resetConfig("close"); //reset configData
      }
    );
  };
  //order, paging --------------------------------------------
  onTableChange = (pagination, filters, sorter) => {
    //get filter data
    let oldFilterData = this.state.filterData;
    let onOrder = { pagination, filters, sorter };
    let filterData = getFilterData(oldFilterData, null, onOrder);
    //get filter data
    this.setState(
      {
        filterData,
        selectedRowKeys: [],
      },
      () => {
        changeUrlFilter(this.state.filterData); //change url
        this.props.getList(this.state.filterData); //get list
        this.resetConfig("close"); //reset configData
      }
    );
  };

  //Delete-----------------------------------------------------
  deleteGroup = () => {
    let NhomNguoiDungID = this.state.selectedRowKeys[0];
    Modal.confirm({
      title: "Xóa dữ liệu",
      content: "Bạn có muốn xóa nhóm người dùng này không?",
      cancelText: "Không",
      okText: "Có",
      onOk: () => {
        api
          .xoaNhom({ NhomNguoiDungID })
          .then((response) => {
            if (response.data.Status > 0) {
              //message success
              message.success("Xóa thành công");
              //reset page
              this.setState({ selectedRowKeys: [] });
              this.props.getList(this.state.filterData);
              this.resetConfig("close");
            } else {
              Modal.error({
                title: "Lỗi",
                content: response.data.Message,
              });
            }
          })
          .catch((error) => {
            Modal.error(Constants.API_ERROR);
          });
      },
    });
  };

  //Modal add ------------------------------------------------
  showModalAddGroup = () => {
    let modalKey = this.state.modalKey + 1;
    this.setState({
      visibleModalAddGroup: true,
      // dataModalAddGroup: {
      //   DanhSachCoQuan: this.props.DanhSachCoQuan,
      //   DanhSachCoQuanID: [],
      // },
      modalKey,
    });
    modal = confirm({
      title: "",
      icon: <i></i>,
      content: (
        <ModalAddGroup
          confirmLoading={this.state.confirmLoading}
          dataModalAddGroup={this.state.dataModalAddGroup}
          onCancel={() => {
            modal.destroy();
            this.props.getList(this.state.filterData);
          }}
          onCreate={this.submitModalAddGroup}
          key={this.state.modalKey}
        />
      ),
    });
  };
  hideModalAddGroup = () => {
    this.setState({
      visibleModalAddGroup: false,
    });
  };
  submitModalAddGroup = (data) => {
    this.setState({ confirmLoading: true }, () => {
      setTimeout(() => {}, 5000);
    });
  };

  //Modal edit ------------------------------------------------
  showModalEditGroup = () => {
    let NhomNguoiDungID = this.state.selectedRowKeys[0];
    api
      .chiTietNhom({ NhomNguoiDungID })
      .then((response) => {
        if (response.data.Status > 0) {
          let modalKey = this.state.modalKey + 1;
          this.setState({
            dataModalEditGroup: {
              DanhSachCoQuan: this.props.DanhSachCoQuan,
              Data: response.data.Data,
            },
            visibleModalEditGroup: true,
            modalKey,
          });
        } else {
          Modal.error({
            title: "Lỗi",
            content: response.data.Message,
          });
        }
      })
      .catch((error) => {
        Modal.error(Constants.API_ERROR);
      });
  };
  hideModalEditGroup = () => {
    this.setState({
      visibleModalEditGroup: false,
      dataModalEditGroup: {
        DanhSachCoQuan: this.props.DanhSachCoQuan,
        DanhSachCoQuanID: [],
        Data: null,
      },
    });
  };
  submitModalEditGroup = (data) => {
    this.setState({ confirmLoading: true }, () => {
      api
        .suaNhom(data)
        .then((response) => {
          this.setState({ confirmLoading: false }, () => {
            if (response.data.Status > 0) {
              //message success
              message.success("Cập nhật thành công");
              //hide modal
              this.hideModalEditGroup();
              //reset page
              this.props.getList(this.state.filterData);
            } else {
              Modal.error({
                title: "Lỗi",
                content: response.data.Message,
              });
            }
          });
        })
        .catch((error) => {
          Modal.error(Constants.API_ERROR);
        });
    });
  };

  //Render action ---------------------------------------------
  renderActionAddGroup = () => {
    return (
      <span>
        <Button type="primary" icon="usergroup-add" onClick={this.showModalAddGroup}>
          Thêm
        </Button>
        {/* <ModalAddGroup confirmLoading={this.state.confirmLoading} visible={this.state.visibleModalAddGroup} dataModalAddGroup={this.state.dataModalAddGroup} onCancel={this.hideModalAddGroup} onCreate={this.submitModalAddGroup} key={this.state.modalKey} /> */}
      </span>
    );
  };
  renderActionEdit = () => {
    return (
      <span>
        <Button type="primary" icon="edit" disabled={this.state.selectedRowKeys.length !== 1} onClick={this.showModalEditGroup}>
          Sửa
        </Button>
        <ModalEditGroup confirmLoading={this.state.confirmLoading} visible={this.state.visibleModalEditGroup} onCancel={this.hideModalEditGroup} onCreate={this.submitModalEditGroup} dataModalEditGroup={this.state.dataModalEditGroup} key={this.state.modalKey} />
      </span>
    );
  };
  renderActionDelete = () => {
    return (
      <span>
        <Button type="primary" icon="delete" disabled={this.state.selectedRowKeys.length !== 1} onClick={this.deleteGroup}>
          Xóa
        </Button>
      </span>
    );
  };

  deleteUser = (param) => {
    Modal.confirm({
      title: "Xóa dữ liệu",
      content: "Bạn có muốn xóa người dùng này ra khỏi nhóm?",
      cancelText: "Không",
      okText: "Có",
      onOk: () => {
        api
          .xoaNguoiDung(param)
          .then((response) => {
            if (response.data.Status > 0) {
              //message success
              message.success("Xóa thành công");
              //reset configData
              this.resetConfig("open");
            } else {
              Modal.error({
                title: "Lỗi",
                content: response.data.Message,
              });
            }
          })
          .catch((error) => {
            Modal.error(Constants.API_ERROR);
          });
      },
    });
  };
  showModalAddUser = () => {
    if (this.state.selectedRowKeys.length === 1) {
      let NhomNguoiDungID = this.state.selectedRowKeys[0];
      api
        .danhSachNguoiDung({ NhomNguoiDungID })
        .then((response) => {
          if (response.data.Status > 0) {
            let modalKey = this.state.modalKey + 1;
            this.setState({
              visibleModalAddUser: true,
              dataModalAddUser: {
                NhomNguoiDungID,
                DanhSachNguoiDung: response.data.Data,
              },
              modalKey,
            });
          } else {
            Modal.error({
              title: "Lỗi",
              content: response.data.Message,
            });
          }
        })
        .catch((error) => Modal.error(Constants.API_ERROR));
    }
  };
  hideModalAddUser = () => {
    this.setState({
      visibleModalAddUser: false,
      dataModalAddUser: {
        NhomNguoiDungID: 0,
        DanhSachNguoiDung: [],
      },
    });
  };
  submitModalAddUser = (data) => {
    this.setState({ confirmLoading: true }, () => {
      api
        .themNguoiDung(data)
        .then((response) => {
          this.setState({ confirmLoading: false }, () => {
            if (response.data.Status > 0) {
              //message success
              message.success("Thêm thành công");
              //hide modal
              this.hideModalAddUser();
              //reset configData
              this.resetConfig("open");
            } else {
              Modal.error({
                title: "Lỗi",
                content: response.data.Message,
              });
            }
          });
        })
        .catch((error) => {
          Modal.error(Constants.API_ERROR);
        });
    });
  };
  renderActionConfig = () => {
    return (
      <span>
        <Button type="primary" icon="setting" disabled={this.state.selectedRowKeys.length !== 1} onClick={() => this.resetConfig("open")}>
          Cấu hình
        </Button>
      </span>
    );
  };
  onChangePermission = (checkedValues, PhanQuyenID) => {
    //(lodash.isEqual(obj1, obj2));
    const Data = { ...this.state.configData };
    //get newPermission
    let newPermission = {
      PhanQuyenID: parseInt(PhanQuyenID),
      Xem: 0,
      Them: 0,
      Sua: 0,
      Xoa: 0,
    };
    let oldPermission = {};
    checkedValues.forEach((item) => {
      newPermission[item] = 1;
    });
    //get oldPermission
    Data.DanhSachChucNang.some((item) => {
      if (item.PhanQuyenID === PhanQuyenID) {
        oldPermission = {
          PhanQuyenID: parseInt(item.PhanQuyenID),
          Xem: parseInt(item.Xem),
          Them: parseInt(item.Them),
          Sua: parseInt(item.Sua),
          Xoa: parseInt(item.Xoa),
        };
        return true;
      }
      return false;
    });
    let permissionsChanged = [...this.state.permissionsChanged];
    permissionsChanged.some((item, index) => {
      if (item.PhanQuyenID === newPermission.PhanQuyenID) {
        permissionsChanged.splice(index, 1);
        return true;
      }
      return false;
    });
    if (!lodash.isEqual(newPermission, oldPermission)) {
      permissionsChanged.push(newPermission);
    }
    this.setState({ permissionsChanged });
  };
  savePermissionsChanged = () => {
    const paramArray = this.state.permissionsChanged;
    if (paramArray && paramArray.length) {
      Modal.confirm({
        title: "Lưu thay đổi",
        content: "Bạn có muốn cập nhật thay đổi không?",
        cancelText: "Không",
        okText: "Có",
        onOk: () => {
          this.setState({ confirmLoading: true });
          api
            .suaChucNang(paramArray)
            .then((response) => {
              if (response.data.Status > 0) {
                //message success
                message.success("Cập nhật thành công");
                this.setState({ permissionsChanged: [] }, () => {
                  //reset configData
                  this.resetConfig("open");
                });
                this.setState({ confirmLoading: false });
              } else {
                Modal.error({
                  title: "Lỗi",
                  content: response.data.Message,
                });
                this.setState({ confirmLoading: false });
              }
            })
            .catch((error) => {
              Modal.error(Constants.API_ERROR);
              this.setState({ confirmLoading: false });
            });
        },
        onCancel: () => {
          this.setState({ permissionsChanged: [] }, () => {
            //reset configData
            this.resetConfig("open");
          });
        },
      });
    }
  };
  renderBoxConfig = () => {
    if (this.state.configData && this.state.visibleConfigGroup) {
      const Data = { ...this.state.configData };
      if (Data) {
        return (
          <Box style={{ marginBottom: 20 }} key={this.state.configKey}>
            <BoxConfig>
              <button className="closeButton" onClick={() => this.resetConfig("close")}>
                ✖
              </button>
              <Row gutter={8}>
                <Col lg={12} md={24}>
                  <Box className="box_class">
                    <h3>Thêm người dùng</h3>
                    <div className="action_class">
                      {this.props.role.add ? (
                        <Button type="primary" icon="user-add" onClick={this.showModalAddUser} style={{ height: 28 }}>
                          Thêm người dùng
                        </Button>
                      ) : (
                        ""
                      )}
                      <ModalAddUser confirmLoading={this.state.confirmLoading} visible={this.state.visibleModalAddUser} dataModalAddUser={this.state.dataModalAddUser} onCancel={this.hideModalAddUser} onCreate={this.submitModalAddUser} key={this.state.modalKey} />
                    </div>
                    <div className="content_class">
                      {Data.DanhSachNguoiDung && Data.DanhSachNguoiDung.length ? (
                        <ul>
                          {Data.DanhSachNguoiDung.map((item, key) => (
                            <li key={key}>
                              {item.TenNguoiDung}
                              <button
                                onClick={() =>
                                  this.deleteUser({
                                    NhomNguoiDungID: Data.NhomNguoiDungID,
                                    NguoiDungID: item.NguoiDungID,
                                    CoQuanID: item.CoQuanID,
                                  })
                                }
                              >
                                ✖
                              </button>
                            </li>
                          ))}
                        </ul>
                      ) : (
                        <EmptyTable />
                      )}
                    </div>
                  </Box>
                </Col>
                <Col lg={12} md={24}>
                  <Box className="box_class">
                    <h3>Thêm chức năng</h3>
                    <div className="action_class">
                      {this.props.role.edit ? (
                        <Button type="primary" icon="save" disabled={!this.state.permissionsChanged.length} onClick={this.savePermissionsChanged} style={{ height: 28 }}>
                          Lưu
                        </Button>
                      ) : (
                        ""
                      )}
                      {this.props.role.edit ? (
                        <Button type="primary" icon="file-add" onClick={this.showModalAddPermission} style={{ height: 28 }}>
                          Thêm chức năng
                        </Button>
                      ) : (
                        ""
                      )}
                      <ModalAddPermission confirmLoading={this.state.confirmLoading} visible={this.state.visibleModalAddPermission} dataModalAddPermission={this.state.dataModalAddPermission} onCancel={this.hideModalAddPermission} onCreate={this.submitModalAddPermission} key={this.state.modalKey} />
                    </div>
                    <div className="content_class" style={{ paddingTop: "20px" }}>
                      {this.renderOptions(Data.DanhSachChucNang)}
                    </div>
                  </Box>
                </Col>
              </Row>
            </BoxConfig>
          </Box>
        );
      }
    }
    return null;
  };
  renderOptions = (DanhSachChucNang) => {
    let DanhSachNhomChucNang = [];
    let optionsComponent = <EmptyTable />;
    if (DanhSachChucNang && DanhSachChucNang.length) {
      optionsSidebar.forEach((group) => {
        if (group.children) {
          let childrenKeys = group.children.map((children) => children.key);
          DanhSachChucNang.some((option) => {
            if (childrenKeys.indexOf(option.MaChucNang) >= 0) {
              DanhSachNhomChucNang.push({
                label: group.label,
                childrenKeys,
                isParent: true,
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
                isParent: option.ChucNangChaID > 0,
              });
              return true;
            }
            return false;
          });
        }
      });
      optionsComponent = DanhSachNhomChucNang.map((groupValue, indexParent) => {
        if (!groupValue.isParent) {
          return DanhSachChucNang.map((item, index) => {
            if (groupValue.key === item.MaChucNang) {
              let options = [
                { label: "Xem", value: "Xem", disabled: false },
                { label: "Thêm", value: "Them", disabled: false },
                { label: "Sửa", value: "Sua", disabled: false },
                { label: "Xóa", value: "Xoa", disabled: false },
              ];
              let defaultValue = [];
              if (item.Xem) defaultValue.push("Xem");
              if (item.Them) defaultValue.push("Them");
              if (item.Sua) defaultValue.push("Sua");
              if (item.Xoa) defaultValue.push("Xoa");
              return (
                <div key={index} className="content_row">
                  <b className="tenchucnang">{item.TenChucNang}</b>
                  <div className="chonxoaquyen">
                    {/* <Checkbox.Group options={options} defaultValue={defaultValue} onChange={(checkedValue) => this.onChangePermission(checkedValue, item.PhanQuyenID)} /> */}
                    {specialPermission.findIndex((d) => d.key === item.MaChucNang) !== -1 ? (
                      <Checkbox.Group defaultValue={defaultValue} options={[{ label: "", value: "Xem" }]} onChange={(checkedValue) => this.onChangePermission(checkedValue, item.PhanQuyenID)}></Checkbox.Group>
                    ) : (
                      <Checkbox.Group options={options} defaultValue={defaultValue} onChange={(checkedValue) => this.onChangePermission(checkedValue, item.PhanQuyenID)} />
                    )}
                    <button
                      className="float-right"
                      onClick={() =>
                        this.deletePermission({
                          PhanQuyenID: item.PhanQuyenID,
                        })
                      }
                    >
                      ✖
                    </button>
                    <div className="clearfix"></div>
                  </div>
                </div>
              );
            }
          });
        } else {
          return (
            <div>
              <div>
                <b>{groupValue.label}</b>
              </div>
              {DanhSachChucNang.map((item, index) => {
                if (groupValue.childrenKeys && groupValue.childrenKeys.indexOf(item.MaChucNang) >= 0) {
                  let options = [],
                    defaultValue = [];
                  const ChucNangID = item.ChucNangID;
                  //get parent item from all list chuc nang
                  let parentItem = null;
                  const DanhSachChucNangCha = this.state.allList;
                  DanhSachChucNangCha.some((pItem) => {
                    if (pItem.ChucNangID === ChucNangID) {
                      parentItem = { ...pItem };
                      return true;
                    }
                    return false;
                  });
                  if (parentItem) {
                    options = [
                      { label: "Xem", value: "Xem", disabled: parentItem.Xem === 0 },
                      { label: "Thêm", value: "Them", disabled: parentItem.Them === 0 },
                      { label: "Sửa", value: "Sua", disabled: parentItem.Sua === 0 },
                      { label: "Xóa", value: "Xoa", disabled: parentItem.Xoa === 0 },
                    ];
                    if (item.Xem) defaultValue.push("Xem");
                    if (item.Them) defaultValue.push("Them");
                    if (item.Sua) defaultValue.push("Sua");
                    if (item.Xoa) defaultValue.push("Xoa");
                    return (
                      <div key={index} className="content_row">
                        <div className="tenchucnang">{item.TenChucNang}</div>
                        <div className="chonxoaquyen">
                          {specialPermission.findIndex((d) => d.key === item.MaChucNang) !== -1 ? (
                            <Checkbox.Group defaultValue={defaultValue} options={[{ label: "", value: "Xem" }]} onChange={(checkedValue) => this.onChangePermission(checkedValue, item.ChucNangID)}></Checkbox.Group>
                          ) : (
                            <Checkbox.Group defaultValue={defaultValue} options={options} onChange={(checkedValue) => this.onChangePermission(checkedValue, item.ChucNangID)} />
                          )}
                          <button
                            className="float-right"
                            onClick={() =>
                              this.deletePermission({
                                PhanQuyenID: item.PhanQuyenID,
                              })
                            }
                          >
                            ✖
                          </button>
                          <div className="clearfix"></div>
                        </div>
                      </div>
                    );
                  }
                }
              })}
            </div>
          );
        }
      });
    }
    return optionsComponent;
  };
  deletePermission = (param) => {
    Modal.confirm({
      title: "Xóa dữ liệu",
      content: "Bạn có muốn xóa chức năng này ra khỏi nhóm?",
      cancelText: "Không",
      okText: "Có",
      onOk: () => {
        api
          .xoaChucNang(param)
          .then((response) => {
            if (response.data.Status > 0) {
              //message success
              message.destroy();
              message.success("Xóa thành công");
              //reset configData
              this.resetConfig("open");
            } else {
              Modal.error({
                title: "Lỗi",
                content: response.data.Message,
              });
            }
          })
          .catch((error) => {
            Modal.error(Constants.API_ERROR);
          });
      },
    });
  };
  showModalAddPermission = () => {
    if (this.state.selectedRowKeys.length === 1) {
      let NhomNguoiDungID = this.state.selectedRowKeys[0];
      let DanhSachChucNang = [];
      //lay tat ca chuc nang nguoi dung co the sd
      const AllList = this.state.allList;
      //danh sach chuc nang da dc sd
      const ExistList = this.state.configData.DanhSachChucNang;
      const ExistID = ExistList && ExistList.length ? ExistList.map((item) => item.ChucNangID.toString()) : [];
      AllList.map((item) => {
        //neu chuc nang nay chua dc sd -> disable = false
        if (ExistID.indexOf(item.ChucNangID.toString()) < 0) {
          DanhSachChucNang.push({ ...item, disabled: false });
        } else {
          DanhSachChucNang.push({ ...item, disabled: true });
        }
      });
      //set state open modal
      let modalKey = this.state.modalKey + 1;
      this.setState({
        visibleModalAddPermission: true,
        dataModalAddPermission: {
          NhomNguoiDungID,
          DanhSachChucNang,
        },
        modalKey,
      });
    }
  };
  hideModalAddPermission = () => {
    this.setState({
      visibleModalAddPermission: false,
      dataModalAddPermission: {
        NhomNguoiDungID: 0,
        DanhSachChucNang: [],
      },
    });
  };
  submitModalAddPermission = (data) => {
    this.setState({ confirmLoading: true }, () => {
      api
        .themChucNang(data)
        .then((response) => {
          this.setState({ confirmLoading: false }, () => {
            if (response.data.Status > 0) {
              //message success
              message.success("Thêm thành công");
              //hide modal
              this.hideModalAddPermission();
              //reset configData
              this.resetConfig("open");
            } else {
              Modal.error({
                title: "Lỗi",
                content: response.data.Message,
              });
            }
          });
        })
        .catch((error) => {
          Modal.error(Constants.API_ERROR);
        });
    });
  };
  resetConfig = (status) => {
    if (status === "open") {
      if (this.state.selectedRowKeys.length === 1) {
        let configKey = this.state.configKey + 1;
        let NhomNguoiDungID = this.state.selectedRowKeys[0];
        api
          .sieuChiTietNhom({ NhomNguoiDungID })
          .then((response) => {
            if (response.data.Status > 0) {
              api.danhSachChucNangDuocThaoTac({ NhomNguoiDungID }).then((response2) => {
                this.setState({
                  visibleConfigGroup: true,
                  configKey,
                  configData: response.data.Data,
                  permissionsChanged: [],
                  allList: response2.data.Data,
                });
              });
            } else {
              Modal.error({
                title: "Lỗi",
                content: response.data.Message,
              });
            }
          })
          .catch((error) => Modal.error(Constants.API_ERROR));
      }
    } else {
      this.setState({
        visibleConfigGroup: false,
        configData: null,
        permissionsChanged: [],
        allList: [],
      });
    }
  };

  onSelectChange = (selectedRowKeys, selectedRows) => {
    let length = selectedRowKeys.length;
    let sr = length ? [selectedRowKeys[length - 1]] : selectedRowKeys;
    if (length && this.state.selectedRowKeys.length && this.state.visibleConfigGroup) {
      //if config div is open and choose other row -> reset selectRow, configBox
      let configKey = this.state.configKey + 1;
      let NhomNguoiDungID = sr[0];
      api
        .sieuChiTietNhom({ NhomNguoiDungID })
        .then((response) => {
          if (response.data.Status > 0) {
            api.danhSachChucNangDuocThaoTac({ NhomNguoiDungID }).then((response2) => {
              // console.log(response, response2);
              this.setState({
                selectedRowKeys: sr,
                visibleConfigGroup: true,
                configKey,
                configData: response.data.Data,
                permissionsChanged: [],
                allList: response2.data.Data, //danh sach chuc nang cha
              });
            });
          } else {
            Modal.error({
              title: "Lỗi",
              content: response.data.Message,
            });
          }
        })
        .catch((error) => Modal.error(Constants.API_ERROR));
    } else {
      //reset selectRow
      this.setState({ selectedRowKeys: sr, visibleConfigGroup: false, allList: [] });
    }
  };

  //Render ----------------------------------------------------
  render() {
    //data
    const { DanhSachNhom, TotalRow, role } = this.props;

    //paging info
    const filterData = this.state.filterData;
    const PageNumber = filterData.PageNumber ? parseInt(filterData.PageNumber) : 1;
    const PageSize = filterData.PageSize ? parseInt(filterData.PageSize) : getDefaultPageSize();
    //format table
    const columns = [
      {
        title: "STT",
        align: "center",
        width: 50,
        render: (text, record, index) => <span>{(PageNumber - 1) * PageSize + (index + 1)}</span>,
      },
      {
        title: "Tên nhóm người dùng",
        dataIndex: "TenNhom",
        key: "TenNhom",
        width: "32%",
      },
      {
        title: "Ghi chú",
        dataIndex: "GhiChu",
        key: "GhiChu",
        // render: (text, record, index) => <span style={{ whiteSpace: "pre" }}>{text}</span>,
        width: "60%",
      },
    ];
    return (
      <LayoutWrapper>
        <PageHeader>Phân quyền </PageHeader>
        <PageAction>
          {role.edit ? (
            <Button type="primary" icon="setting" style={{ marginRight: 5 }} disabled={this.state.selectedRowKeys.length !== 1} onClick={() => this.resetConfig("open")}>
              Cấu hình
            </Button>
          ) : (
            ""
          )}
          {role.edit ? (
            <Button type="primary" icon="edit" style={{ marginRight: 5 }} disabled={this.state.selectedRowKeys.length !== 1} onClick={this.showModalEditGroup}>
              Sửa
            </Button>
          ) : (
            ""
          )}
          {role.delete ? (
            <Button type="primary" icon="delete" style={{ marginRight: 5 }} disabled={this.state.selectedRowKeys.length !== 1} onClick={this.deleteGroup}>
              Xóa
            </Button>
          ) : (
            ""
          )}
          {role.add ? (
            <Button type="primary" icon="usergroup-add" style={{ marginRight: 5 }} onClick={this.showModalAddGroup}>
              Thêm
            </Button>
          ) : (
            ""
          )}
        </PageAction>
        <ModalEditGroup confirmLoading={this.state.confirmLoading} visible={this.state.visibleModalEditGroup} onCancel={this.hideModalEditGroup} onCreate={this.submitModalEditGroup} dataModalEditGroup={this.state.dataModalEditGroup} key={this.state.modalKey} />

        {this.renderBoxConfig()}
        <Box>
          <BoxFilter>
            <Input.Search allowClear defaultValue={this.state.filterData.Keyword} placeholder="Tìm kiếm theo tên nhóm người dùng" onSearch={(value) => this.onFilter(value, "Keyword")} style={{ width: 300 }} />
          </BoxFilter>
          <BoxTableDiv>
            <BoxTable
              rowSelection={{
                onChange: this.onSelectChange,
                selectedRowKeys: this.state.selectedRowKeys,
                columnWidth: 50,
              }}
              columns={columns}
              rowKey="NhomNguoiDungID"
              dataSource={DanhSachNhom}
              loading={this.props.TableLoading}
              onChange={this.onTableChange}
              pagination={{
                showSizeChanger: true, //show text: PageSize/page
                showTotal: (total, range) => `Từ ${range[0]} đến ${range[1]} trên ${total} kết quả`,
                total: TotalRow,
                current: PageNumber, //current page
                pageSize: PageSize,
              }}
              // scroll={{ y: "65vh" }}
            />
          </BoxTableDiv>
        </Box>
      </LayoutWrapper>
    );
  }
}

function mapStateToProps(state) {
  return {
    ...state.QLPhanQuyen,
  };
}

export default connect(mapStateToProps, actions)(QLPhanQuyen);
