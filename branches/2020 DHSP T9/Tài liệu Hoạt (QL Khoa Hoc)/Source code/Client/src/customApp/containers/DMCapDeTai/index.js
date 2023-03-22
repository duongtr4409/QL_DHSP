import React, { Component } from "react";
import { connect } from "react-redux";
import queryString from "query-string";
import actions from "../../redux/DMCapDeTai/actions";
import api from "./config";
import apiCanBo from "../QLTaiKhoan/config";
import Constants from "../../../settings/constants";

import LayoutWrapper from "../../../components/utility/layoutWrapper";
import PageHeader from "../../../components/utility/pageHeader";
import PageAction from "../../../components/utility/pageAction";
import Box from "../../../components/utility/box";
import BoxFilter from "../../../components/utility/boxFilter";
import { EmptyTable } from "../../../components/utility/boxTable";
import { getRoleByKey2 } from "../../../helpers/utility";
import { ModalAdd } from "./modalAdd";

import { Modal, message, Input, Tree, Menu, Dropdown, Icon, Tooltip } from "antd";
import Button from "../../../components/uielements/button";
import { changeUrlFilter, getFilterData } from "../../../helpers/utility";

const { TreeNode } = Tree;

class DMCapDeTai extends Component {
  constructor(props) {
    super(props);
    document.title = "Danh mục cấp đề tài";
    const filterData = queryString.parse(this.props.location.search);
    this.state = {
      expandedKeys: [],
      filterData: { ...filterData },
      treeKey: 0,

      DanhSachCapDeTai: [],
      confirmLoading: false,
    };
    this.modal = null;
  }

  //Get initData---------------------------------------------
  componentDidMount = () => {
    this.props.getInitData(this.state.filterData);
  };

  componentDidUpdate(prevProps, prevState, snapshot) {
    if (prevProps.DanhSachCapDeTai !== this.props.DanhSachCapDeTai) {
      let treeKey = this.state.treeKey + 1;
      this.setState({
        DanhSachCapDeTai: this.props.DanhSachCapDeTai,
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

  deleteData = (ID) => {
    if (false) {
      message.destroy();
      message.warning("Bạn không có quyền thực hiện chức năng này");
    } else {
      Modal.confirm({
        title: "Xóa dữ liệu",
        content: "Bạn có muốn xóa cấp đề tài này không?",
        cancelText: "Không",
        okText: "Có",
        onOk: () => {
          api
            .xoaCapDeTai([ID])
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
    this.modal.destroy();
    this.props.getList(this.state.filterData);
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
              <span>Thêm cấp đề tài</span>
            </Menu.Item>
          ) : null}
          {this.props.role.edit ? (
            <Menu.Item onClick={this.showModal(parentNode, item)}>
              <span>Sửa</span>
            </Menu.Item>
          ) : null}
          {this.props.role.edit ? (
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
                <span className=" text-truncate " style={{ width: Tree.clientWidth - 200 }}>
                  {item.Name}
                </span>
              </Tooltip>
            </Dropdown>
          </div>
        ) : (
          <div>
            <Dropdown overlay={menu} placement="bottomLeft" trigger={["contextMenu"]}>
              <Tooltip title={item.Name} mouseEnterDelay={0.5}>
                <b className=" text-truncate " style={{ width: Tree.clientWidth - 200 }}>
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
      return <TreeNode title={title} key={item.Id} isLeaf={false} children={item.Children} dataRef={item} />;
    });
  };

  renderContent = () => {
    if (this.state.DanhSachCapDeTai.length) {
      return (
        <Tree defaultExpandAll={true} autoExpandParent showLine switcherIcon={<Icon type="down" />} filterTreeNode={(treeNode) => treeNode.props.dataRef.Highlight === 1} onSelect={this.onExpandNode} onExpand={this.onExpandNode}>
          {this.renderTreeNodes(this.state.DanhSachCapDeTai)}
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
        <PageHeader>Danh mục cấp đề tài</PageHeader>
        <PageAction>
          {this.props.role.add ? (
            <Button type="primary" icon="file-add" onClick={this.showModal()}>
              Thêm
            </Button>
          ) : (
            ""
          )}
        </PageAction>
        <Box style={{ minHeight: "calc(100vh - 265px)" }}>
          <BoxFilter>
            <Input.Search allowClear={true} defaultValue={this.state.filterData.Keyword} placeholder="Tìm kiếm theo tên cấp đề tài" onSearch={(value) => this.onFilter(value, "Keyword")} style={{ width: 300 }} />
          </BoxFilter>
          <div key={this.state.treeKey} style={{ userSelect: "none" }} id="tree">
            {this.renderContent()}
          </div>
        </Box>
      </LayoutWrapper>
    );
  }
}

function mapStateToProps(state) {
  const role = getRoleByKey2("dm-cap-de-tai");

  state.DMCapDeTai.role = role;
  return {
    ...state.DMCapDeTai,
  };
}

export default connect(mapStateToProps, actions)(DMCapDeTai);
