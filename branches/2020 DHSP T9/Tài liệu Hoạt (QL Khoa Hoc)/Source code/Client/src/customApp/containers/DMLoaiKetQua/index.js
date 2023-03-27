import React, {Component} from "react";
import {connect} from "react-redux";
import queryString from "query-string";
import actions from "../../redux/DMLoaiKetQua/actions";
import api from "./config";
import Constants from "../../../settings/constants";
import LayoutWrapper from "../../../components/utility/layoutWrapper";
import PageHeader from "../../../components/utility/pageHeader";
import PageAction from "../../../components/utility/pageAction";
import Box from "../../../components/utility/box";
import BoxFilter from "../../../components/utility/boxFilter";
import {EmptyTable} from "../../../components/utility/boxTable";
import {changeUrlFilter, getFilterData, getRoleByKey} from "../../../helpers/utility";
import {ModalAddEdit} from "./modalAddEditLoaiKetQua";
import {Dropdown, Icon, Input, Menu, message, Modal, Tooltip, Tree} from "antd";
import Button from "../../../components/uielements/button";
import './style.css';

const {TreeNode} = Tree;

class DMLoaiKetQua extends Component {
  constructor(props) {
    super(props);
    document.title = "Danh mục loại kết quả";
    const filterData = queryString.parse(this.props.location.search);
    this.state = {
      expandedKeys: [],
      filterData: {...filterData},
      treeKey: 0,

      DanhSachLoaiKetQua: [],
      loading: false,
      visibleModal: false,
      modalKey: 0,
      actions: 1,
      dataEdit: {}
    };
  }

  //Get initData---------------------------------------------
  componentDidMount = () => {
    this.props.getInitData(this.state.filterData);
  };

  componentDidUpdate(prevProps, prevState, snapshot) {
    if (prevProps.DanhSachLoaiKetQua !== this.props.DanhSachLoaiKetQua) {
      let treeKey = this.state.treeKey + 1;
      this.setState({
        DanhSachLoaiKetQua: this.props.DanhSachLoaiKetQua,
        expandedKeys: this.props.expandedKeys,
        treeKey,
      });
    }
  }

  onFilter = (value, property) => {
    let oldFilterData = {...this.state.filterData};
    if(typeof value == 'string')
    {
      value = value.trim();
    }
    let onFilter = {value, property};
    let filterData = getFilterData(oldFilterData, onFilter, null);
    this.setState(
      {
        filterData,
      },
      () => {
        let Keyword = this.state.filterData.Keyword ? this.state.filterData.Keyword : "";
        changeUrlFilter({Keyword}); //change url
        this.props.getList(this.state.filterData); //get list
      }
    );
  };

  deleteData = (ID) => {
    Modal.confirm({
      title: "Xóa dữ liệu",
      content: "Bạn có muốn xóa loại kết quả này không?",
      cancelText: "Không",
      okText: "Có",
      onOk: () => {
        api.XoaLoaiKetQua([ID])
          .then((response) => {
            if (response.data.Status > 0) {
              //reset tree
              this.props.getList(this.state.filterData); //get list
              //message success
              message.destroy();
              message.success("Xóa thành công");
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

  handleCloseModal = () => {
    this.setState({visibleModal: false});
  };

  showModal = (parentNode = null, item = null, type) => {
    // console.log(parentNode, item);
    //type 1 - Add new parent 2 - Add new child 3 - Edit parent/child
    let {modalKey} = this.state;
    modalKey++;
    let dataEdit = {
      parentNode: parentNode,
      item: item
    };
    this.setState({visibleModal: true, modalKey, actions: type, dataEdit, loading: false});
  };

  submitModal = (value) => {
    const {actions} = this.state;
    this.setState({loading: true});
    if (actions === 1) {
      delete value.Id;
      api.ThemLoaiKetQua(value).then(response => {
        this.setState({loading: false});
        if (response.data.Status > 0) {
          message.destroy();
          message.success('Thêm mới loại kết quả thành công');
          this.setState({visibleModal: false});
          this.props.getList(this.state.filterData);
        } else {
          message.destroy();
          message.error(response.data.Message);
        }
      }).catch(error => {
        this.setState({loading: false});
        message.destroy();
        message.error(error.toString());
      })
    } else if (actions === 2) {
      if (value.isAddNew) {
        delete value.Id;
        api.ThemLoaiKetQua(value).then(response => {
          this.setState({loading: false});
          if (response.data.Status > 0) {
            message.destroy();
            message.success('Thêm mới loại kết quả thành công');
            this.setState({visibleModal: false});
            this.props.getList(this.state.filterData);
          } else {
            message.destroy();
            message.error(response.data.Message);
          }
        }).catch(error => {
          this.setState({loading: false});
          message.destroy();
          message.error(error.toString());
        })
      } else {
        let danhSachNhiemVu = [];
        value.ListNhiemVuSelected.forEach(item => {
          const {Name, Id} = item;
          danhSachNhiemVu.push({
            Name,
            ParentId: value.ParentId,
            MappingId: Id,
            Status: value.Status,
          });
        });
        api.ThemNhieuLoaiKetQua({items: danhSachNhiemVu}).then(response => {
          this.setState({loading: false});
          if (response.data.Status > 0) {
            message.destroy();
            message.success('Thêm mới loại kết quả thành công');
            this.setState({visibleModal: false});
            this.props.getList(this.state.filterData);
          } else {
            message.destroy();
            message.error(response.data.Message);
          }
        }).catch(error => {
          this.setState({loading: false});
          message.destroy();
          message.error(error.toString());
        })
      }
    } else if (actions === 3) {
      api.SuaLoaiKetQua(value).then(response => {
        this.setState({loading: false});
        if (response.data.Status > 0) {
          message.destroy();
          message.success('Cập nhật loại kết quả thành công');
          this.setState({visibleModal: false});
          this.props.getList(this.state.filterData);
        } else {
          message.destroy();
          message.error(response.data.Message);
        }
      }).catch(error => {
        this.setState({loading: false});
        message.destroy();
        message.error(error.toString());
      })
    }
  };

  onExpandNode = (selectedKeys, info) => {
    let className = info.nativeEvent.target.outerHTML.toString();
    let parentClassName = info.nativeEvent.target.parentElement.className.toString();
    let checkMenu = className.includes("ant-dropdown-menu");
    let checkNearMenu = parentClassName.includes("ant-dropdown-menu");
    if (!checkMenu && !checkNearMenu) {
      //neu dang k click menu drop
      let key = info.node.props.eventKey.toString();
      if (key) {
        if (!info.node.props.isLeaf) {
          let expandedKeys = [...this.state.expandedKeys];
          let index = expandedKeys.indexOf(key);
          if (index > -1) {
            expandedKeys.splice(index, 1);
          } else {
            expandedKeys = this.state.expandedKeys.concat([key]);
          }
          this.setState({expandedKeys});
        }
      }
    }
  };

  renderTreeNodes = (data, parentNode = null) => {
    const Tree = document.getElementById("tree");
    const {role} = this.props;
    return data.map((item) => {
      const user_id = parseInt(localStorage.getItem("user_id"));
      let menu = (
        <Menu>
          {role.add ? ( //if Cap = 1 or 2
            <Menu.Item onClick={() => this.showModal(item, item, 2)}>
              <span>Thêm loại kết quả</span>
            </Menu.Item>
          ) : null}
          {role.edit ? (
            <Menu.Item onClick={() => this.showModal(parentNode, item, 3)}>
              <span>Sửa</span>
            </Menu.Item>
          ) : null}
          {role.edit ? (
            <Menu.Item
              onClick={() => {
                if (item.Children.length !== 0) {
                  message.warning("Vui lòng xoá hết danh mục con trước");
                } else {
                  this.deleteData(item.Id);
                }
              }}
              disabled={item.Cap === 1 && user_id !== 1}
            >
              <span>Xóa</span>
            </Menu.Item>
          ) : null}
        </Menu>
      );
      let title =
        1 === 1 ? (
          <div>
            <Dropdown overlay={menu} placement="bottomLeft" trigger={["contextMenu"]}>
              <Tooltip title={item.Name} mouseEnterDelay={0.5}>
                <span className=" text-truncate " style={{width: Tree ? Tree.clientWidth - 200 : 0}}>
                  {item.Name}
                </span>
              </Tooltip>
            </Dropdown>
          </div>
        ) : (
          <div>
            <Dropdown overlay={menu} placement="bottomLeft" trigger={["contextMenu"]}>
              <Tooltip title={item.Name} mouseEnterDelay={0.5}>
                <b className=" text-truncate " style={{width: Tree ? Tree.clientWidth - 200 : 0}}>
                  {item.Name}
                </b>
              </Tooltip>
            </Dropdown>
          </div>
        );

      if (item.Children) {
        return (
          <TreeNode title={title} key={item.Id} isLeaf={false} children={item.Children} dataRef={item}>
            {this.renderTreeNodes(item.Children, item)}
          </TreeNode>
        );
      }
      return <TreeNode title={title} key={item.Id} isLeaf={false} children={item.Children} dataRef={item}/>;
    });
  };

  renderContent = () => {
    if (this.props.DanhSachLoaiKetQua.length) {
      return (
        <Tree defaultExpandAll={true} autoExpandParent showLine switcherIcon={<Icon type="down"/>}
              filterTreeNode={(treeNode) => treeNode.props.dataRef.Highlight === 1} onSelect={this.onExpandNode}
              onExpand={this.onExpandNode}>
          {this.renderTreeNodes(this.props.DanhSachLoaiKetQua)}
        </Tree>
      );
    } else {
      return <EmptyTable loading={this.props.TableLoading}/>;
    }
  };

  render() {
    const {role, DanhSachNhiemVu} = this.props;
    const {actions, visibleModal, modalKey, dataEdit, loading, treeKey} = this.state;
    return (
      <LayoutWrapper>
        <PageHeader>Danh mục loại kết quả</PageHeader>
        <PageAction>
          {role.add ? (
            <Button type="primary" icon="file-add" onClick={() => this.showModal(null, null, 1)}>
              Thêm
            </Button>
          ) : (
            ""
          )}
        </PageAction>
        <Box style={{minHeight: "calc(100vh - 265px)"}}>
          <BoxFilter>
            <Input.Search allowClear={true} defaultValue={this.state.filterData.Keyword}
                          placeholder="Tìm kiếm theo tên loại kết quả"
                          onSearch={(value) => this.onFilter(value, "Keyword")} style={{width: 300}}/>
          </BoxFilter>
          <div key={treeKey} style={{userSelect: "none"}} id="tree">
            {this.renderContent()}
          </div>
        </Box>
        <ModalAddEdit actions={actions} visible={visibleModal} key={modalKey} dataEdit={dataEdit}
                      onCancel={this.handleCloseModal} onCreate={this.submitModal} loading={loading}
                      DanhSachNhiemVu={DanhSachNhiemVu}/>
      </LayoutWrapper>
    );
  }
}

function mapStateToProps(state) {
  state.DMLoaiKetQua.role = getRoleByKey("dm-loai-ket-qua");
  return {
    ...state.DMLoaiKetQua,
  };
}

export default connect(mapStateToProps, actions)(DMLoaiKetQua);
