import React, {Component} from "react";
import {Tooltip, TreeSelect} from "antd";

const {TreeNode} = TreeSelect;

export default class TreeSelectEllipsis extends Component {
  renderTreeNode = (data, disabledCategoryId) => {
    return data.map((item) => {
      if (item.Children) {
        return (
          <TreeNode
            value={item.Id}
            title={
              <Tooltip title={item.Name} mouseEnterDelay={0.5}>
                  <span className="d-inline-block text-truncate " style={{width: "100%", transform: "translateY(4px)"}}>
                    {item.Name}
                  </span>
              </Tooltip>
            }
            key={item.Id}
            searchKey={item.Name}
            disabled={disabledCategoryId ? item.CategoryId === 0 : false}
          >
            {this.renderTreeNode(item.Children, disabledCategoryId)}
          </TreeNode>
        );
      } else {
        return (
          <TreeNode
            value={item.Id}
            title={
              <Tooltip title={item.Name} mouseEnterDelay={0.5}>
                  <span className="d-inline-block text-truncate" style={{width: "100%", transform: "translateY(4px)"}}>
                    {item.Name}
                  </span>
              </Tooltip>
            }
            key={item.Id}
            searchKey={item.Name}
            disabled={disabledCategoryId ? item.CategoryId === 0 : false}
          />
        );
      }
    });
  };

  render() {
    return (
      <TreeSelect {...this.props}
                  dropdownStyle={{maxWidth: 400, overflowX: "hidden", maxHeight: 400}}
                  allowClear
                  showSearch
                  defaultExpandParent
                  filterTreeNode treeNodeFilterProp="searchKey"
      >
        {this.renderTreeNode(this.props.data, this.props.disabledCategoryId)}
      </TreeSelect>
    )
  }
}