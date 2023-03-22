import React, { Component } from "react";
import { connect } from "react-redux";
import queryString from "query-string";
import actions from "../../redux/DMLinhVuc/actions";
import api from "./config";
import apiCanBo from "../QLTaiKhoan/config";
import Constants from "../../../settings/constants";

import LayoutWrapper from "../../../components/utility/layoutWrapper";
import PageHeader from "../../../components/utility/pageHeader";
import PageAction from "../../../components/utility/pageAction";
import Box from "../../../components/utility/box";
import BoxFilter from "../../../components/utility/boxFilter";
import { EmptyTable } from "../../../components/utility/boxTable";

import { ModalAdd } from "./modalAdd";

import { Modal, message, Input, Tree, Menu, Dropdown, Icon, Tooltip } from "antd";
import Button from "../../../components/uielements/button";
import { changeUrlFilter, getFilterData } from "../../../helpers/utility";
import { getRoleByKey2 } from "../../../helpers/utility";

const { TreeNode } = Tree;

class DMLinhVuc extends Component {
  constructor(props) {
    super(props);
    document.title = "Danh mục lĩnh vực";
    const filterData = queryString.parse(this.props.location.search);
    this.state = {
      expandedKeys: [],
      filterData: { ...filterData },
      treeKey: 0,

      DanhSachLinhVuc: [],
      confirmLoading: false,
    };
    this.modal = null;
  }

  //Get initData---------------------------------------------
  componentDidMount = () => {
    this.props.getInitData(this.state.filterData);
  };

  componentDidUpdate(prevProps, prevState, snapshot) {
    if (prevProps.DanhSachLinhVuc !== this.props.DanhSachLinhVuc) {
      let treeKey = this.state.treeKey + 1;
      this.setState({
        DanhSachLinhVuc: this.props.DanhSachLinhVuc,
        expandedKeys: this.props.expandedKeys,
        treeKey,
      });
    }
  }

  onFilter = (value, property) => {
    let oldFilterData = { ...this.state.filterData };
    let onFilter = { value, property };
    let filterData = getFilterData(oldFilterData, onFilter, null);
    this.setState(
      {
        filterData,
      },
      () => {
        let Keyword = this.state.filterData.Keyword ? this.state.filterData.Keyword : "";
        changeUrlFilter({ Keyword }); //change url
        this.props.getList(this.state.filterData); //get list
      }
    );
  };

  deleteData = (ID) => () => {
    if (!this.props.role.delete) {
      message.destroy();
      message.warning("Bạn không có quyền thực hiện chức năng này");
    } else {
      Modal.confirm({
        title: "Xóa dữ liệu",
        content: "Bạn có muốn xóa lĩnh vực này không?",
        cancelText: "Không",
        okText: "Có",
        onOk: () => {
          api
            .xoaLinhVuc([ID])
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
    }
  };

  handleCloseModal = () => {
    this.props.getList(this.state.filterData);
    this.modal.destroy();
  };

  showModal = (parentNode = null, editNode = null) => () => {
    this.modal = Modal.confirm({
      icon: <i />,
      content: <ModalAdd parentNode={parentNode} editNode={editNode} onClose={this.handleCloseModal}></ModalAdd>,
    });
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
          this.setState({ expandedKeys });
        }
      }
    }
  };
  renderTreeNodes = (data, parentNode = null) => {
    const Tree = document.getElementById("tree");
    return data.map((item) => {
      const user_id = parseInt(localStorage.getItem("user_id"));
      let menu = (
        <Menu>
          {this.props.role.add ? ( //if Cap = 1 or 2
            <Menu.Item onClick={this.showModal(item)}>
              <span>Thêm lĩnh vực</span>
            </Menu.Item>
          ) : null}
          {this.props.role.edit ? (
            <Menu.Item onClick={this.showModal(parentNode, item)}>
              <span>Sửa</span>
            </Menu.Item>
          ) : null}
          {this.props.role.delete ? (
            <Menu.Item onClick={this.deleteData(item.Id)} disabled={item.Cap === 1 && user_id !== 1}>
              <span>Xóa</span>
            </Menu.Item>
          ) : null}
        </Menu>
      );
      let title =
        1 === 1 ? (
          <div className=" text-truncate ">
            <Dropdown overlay={menu} placement="bottomLeft" trigger={["contextMenu"]}>
              <Tooltip title={item.Name} mouseEnterDelay={0.5}>
                <span className=" text-truncate " style={{ width: Tree.clientWidth - 200 }}>
                  {item.Name} {`(${item.Code})`}
                </span>
              </Tooltip>
            </Dropdown>
          </div>
        ) : (
          <div className=" text-truncate  ">
            <Dropdown overlay={menu} placement="bottomLeft" trigger={["contextMenu"]}>
              <Tooltip title={item.Name} mouseEnterDelay={0.5}>
                <b className=" text-truncate " style={{ width: Tree.clientWidth - 200 }}>
                  {item.Name} {`(${item.Code})`}
                </b>
              </Tooltip>
            </Dropdown>
          </div>
        );
      if (item.Children) {
        return (
          <TreeNode title={title} key={item.key} isLeaf={false} children={item.Children} dataRef={item}>
            {this.renderTreeNodes(item.Children, item)}
          </TreeNode>
        );
      }
      return <TreeNode title={title} key={item.key} isLeaf={item.isLeaf} children={item.children} dataRef={item} />;
    });
  };

  renderContent = () => {
    if (this.state.DanhSachLinhVuc.length) {
      return (
        <Tree
          id="tree2"
          showLine
          switcherIcon={<Icon type="down" />}
          filterTreeNode={(treeNode) => {
            return treeNode.props.dataRef.Highlight === 1;
          }}
          defaultExpandAll={true}
          onSelect={this.onExpandNode}
          onExpand={this.onExpandNode}
        >
          {this.renderTreeNodes(this.state.DanhSachLinhVuc)}
        </Tree>
      );
    } else {
      return <EmptyTable loading={this.props.TableLoading} />;
    }
  };

  render() {
    const { role, user_id } = this.props;

    return (
      <LayoutWrapper>
        <PageHeader>Danh mục lĩnh vực</PageHeader>
        <PageAction>
          {role.add ? (
            <Button type="primary" icon="file-add" onClick={this.showModal()}>
              Thêm
            </Button>
          ) : null}
        </PageAction>
        <Box style={{ minHeight: "calc(100vh - 265px)" }}>
          <BoxFilter>
            <Input.Search allowClear={true} defaultValue={this.state.filterData.Keyword} placeholder="Tìm kiếm theo tên lĩnh vực" onSearch={(value) => this.onFilter(value, "Keyword")} style={{ width: 300 }} />
          </BoxFilter>
          <div key={this.state.treeKey} style={{ userSelect: "none", width: "100%" }} id="tree">
            {this.renderContent()}
          </div>
        </Box>
      </LayoutWrapper>
    );
  }
}

function mapStateToProps(state) {
  const role = getRoleByKey2("dm-linh-vuc");

  state.DMLinhVuc.role = role;
  return {
    ...state.DMLinhVuc,
  };
}

export default connect(mapStateToProps, actions)(DMLinhVuc);
