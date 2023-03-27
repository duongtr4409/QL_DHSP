/**
 *
 * GoInput
 *
 */

import React from "react";
import { ValidatorComponent } from "react-form-validator-core";
import { Select } from "antd";
import { Modal } from "antd";
import api from "../../containers/Dashboard/config";
import * as lodash from "lodash";
import { withInputToText } from "../withInputToText";
import "./antd4.css";

const { Option } = Select;

// import PropTypes from 'prop-types';
// import styled from 'styled-components';

/* eslint-disable react/prefer-stateless-function */
class GoSelect extends ValidatorComponent {
  renderOption = (item) => {
    const { customOption } = this.props;
    if (!customOption) {
      return item.text;
    }
    return customOption(item);
  };
  onChange = (value) => {
    if (this.props.mode === "multiple") {
      // const formatValue = va
      const options = lodash.intersectionBy(
        this.props.data,
        value.map((item) => ({ value: item })),

        "value"
      );
      this.props.onChange(value, options);
    } else {
      const option = this.props.data.find((d) => d.value == value);
      this.props.onChange(value, option);
    }
  };
  renderValidatorComponent() {
    const { errorMessages, validators, requiredError, validatorListener, onChange, value, hideCondition, id, ...rest } = this.props;

    let data = this.props.data || [];
    if (hideCondition) {
      data = this.props.data.filter((d) => d[hideCondition.field] === hideCondition.value);
    }

    const thisSelect = document.getElementById(this.props.id);
    // console.log(this.props.id);
    if (this.props.withTextToInput && thisSelect) {
      thisSelect.classList.remove("text-to-input-focus");
      thisSelect.classList.add("text-to-input-blur");
    }

    const { isValid } = this.state;

    return (
      <div id={id}>
        <Select
          allowClear
          className={isValid ? "" : "go-select-invalid"}
          value={value === 0 ? null : value}
          notFoundContent="Không có dữ liệu"
          showSearch
          filterOption
          optionFilterProp="label"
          optionLabelProp="label"
          onChange={this.props.returnFullItem ? this.onChange : onChange}
          // style={{ width: "100%" }}

          data={data}
          {...rest}
          ref={(r) => {
            this.input = r;
          }}
        >
          {/* {data.map((d, index) => {
            return (
              <Option key={index} label={d.text} value={d.value}>
                {this.renderOption(d)}
              </Option>
            );
          })} */}
        </Select>
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

export default withInputToText(GoSelect);
