/**
 *
 * GoInput
 *
 */

import React from "react";
import { ValidatorComponent } from "react-form-validator-core";
import { TreeSelect } from "antd";
import { Modal, Tooltip } from "antd";
import api from "../../containers/Dashboard/config";
import * as lodash from "lodash";
import { getFlatDataFromTree } from "../../../helpers/tree-helper";

const { TreeNode } = TreeSelect;
// import PropTypes from 'prop-types';
// import styled from 'styled-components';

/* eslint-disable react/prefer-stateless-function */
class GoSelect extends ValidatorComponent {
  flatTree = [];
  onChange = (value) => {
    const item = this.flatTree.find((d) => d.Id === value);
    this.props.onChange(value, item);
  };
  renderTreeNode = (data) => {
    return data.map((item) => {
      this.flatTree.push(item);
      if (item.Children) {
        return (
          <TreeNode
            value={item.Id}
            title={
              this.props.customTitle ? (
                this.props.customTitle(item)
              ) : (
                // item.Name
                <Tooltip title={item.Name} mouseEnterDelay={0.5}>
                  <span className="d-inline-block text-truncate " style={{ width: "100%", transform: "translateY(4px)" }}>
                    {this.props.apiConfig.codeField ? item[this.props.apiConfig.codeField] + " - " + item.Name : item.Name}
                  </span>
                </Tooltip>
              )
            }
            searchKey={`${this.props.apiConfig.codeField ? `${item[this.props.apiConfig.codeField]} - ` : ""}${item.Name}`}
            key={item.Id}
          >
            {this.renderTreeNode(item.Children)}
          </TreeNode>
        );
      } else {
        return (
          <TreeNode
            value={item.Id}
            title={
              this.props.customTitle ? (
                this.props.customTitle(item)
              ) : (
                // item.Name
                <Tooltip title={item.Name} mouseEnterDelay={0.5}>
                  <span className="d-inline-block text-truncate" style={{ width: 240, transform: "translateY(4px)" }}>
                    {this.props.apiConfig.codeField ? item[this.props.apiConfig.codeField] + " - " + item.Name : item.Name}
                  </span>
                </Tooltip>
              )
            }
            searchKey={`${this.props.apiConfig.codeField ? `${item[this.props.apiConfig.codeField]} - ` : ""}${item.Name}`}
            key={item.Id}
          ></TreeNode>
        );
      }
    });
  };

  renderValidatorComponent() {
    const { errorMessages, validators, requiredError, validatorListener, onChange, id, value, ...rest } = this.props;
    let data = this.props.data || [];

    const { isValid } = this.state;
    const thisSelect = document.getElementById(this.props.id);
    // console.log(this.props.id);
    if (this.props.withTextToInput && thisSelect) {
      thisSelect.classList.remove("text-to-input-focus");
      thisSelect.classList.add("text-to-input-blur");
    }

    return (
      <div id={id}>
        <TreeSelect
          dropdownClassName="customTreeSelect"
          className={isValid ? "" : "go-select-invalid"}
          value={value === 0 ? null : value}
          onChange={this.props.returnFullItem ? this.onChange : onChange}
          dropdownStyle={{ maxWidth: 400, overflowX: "hidden", maxHeight: 400 }}
          allowClear
          showSearch
          defaultExpandParent
          filterTreeNode
          treeNodeFilterProp="searchKey"
          // onSearch={this.props.onSearch}
          {...rest}
          ref={(r) => {
            this.input = r;
          }}
          onFocus={() => {
            if (this.props.withTextToInput && thisSelect) {
              thisSelect.classList.remove("text-to-input-blur");
              thisSelect.classList.add("text-to-input-focus");
            }
          }}
          onBlur={() => {
            if (this.props.withTextToInput && thisSelect) {
              thisSelect.classList.remove("text-to-input-focus");
              thisSelect.classList.add("text-to-input-blur");
            }
          }}
        >
          {this.renderTreeNode(data)}
        </TreeSelect>
        {this.errorText()}
      </div>
    );
  }
  errorText() {
    const { isValid } = this.state;

    if (isValid) {
      return null;
    }

    return (
      <div className="invalid-error" style={{ color: "red" }}>
        {this.getErrorMessage()}
      </div>
    );
  }
}

export default GoSelect;
