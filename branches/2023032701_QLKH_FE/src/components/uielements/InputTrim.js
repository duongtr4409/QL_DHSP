import React, { Component } from "react";
import { Input, InputNumber, Select } from "antd";

class InputTrim extends Component {
  state = {
    value: this.props.value
  };
  catChuoi = e => {
    e.target.value
      .split(" ")
      .filter(item => item)
      .join(" ");
    const key = e.charCode;
    if (
      (key === 32 && e.target.value[e.target.value.length - 1] === " ") ||
      (key === 32 && e.target.value.length === 0)
    ) {
      e.preventDefault();
    }
  };
  componentDidUpdate(prevProps, prevState, snapshot) {
    if (this.props.value !== prevState.value) {
      this.setState({ value: this.props.value });
    }
  }
  render() {
    return (
      <Input
        onKeyPress={this.catChuoi}
        value={this.state.value}
        onChange={this.props.onChange}
        placeholder={this.props.placeholderTrim}
        autoFocus={this.props.autoFocusTrim}
        type={this.props.typeTrim}
        disabled={this.props.disabledTrim}
        id={this.props.idTrim}
      />
    );
  }
}
class InputTrimTextArea extends Component {
  state = {
    value: this.props.value
  };
  catChuoi = e => {
    e.target.value.trim().replace(/ +/g, " ");
    const key = e.charCode;
    if (
      (key === 32 && e.target.value[e.target.value.length - 1] === " ") ||
      (key === 32 && e.target.value.length === 0)
    ) {
      e.preventDefault();
    }
  };
  componentDidUpdate(prevProps, prevState, snapshot) {
    if (this.props.value !== prevState.value) {
      this.setState({ value: this.props.value });
    }
  }
  render() {
    return (
      <Input.TextArea
        onKeyPress={this.catChuoi}
        value={this.state.value}
        onChange={this.props.onChange}
        placeholder={this.props.placeholderTrim}
        autoFocus={this.props.autoFocusTrim}
      />
    );
  }
}

class DropDownYear extends Component {
  state = {
    value: this.props.value
  };
  componentDidUpdate(prevProps, prevState, snapshot) {
    if (this.props.value !== prevState.value) {
      this.setState({ value: this.props.value });
    }
  }
  render() {
    let minOffset = 0,
      maxOffset = 100;
    let thisYear = new Date().getFullYear();
    let allYears = [];
    for (let x = minOffset; x <= maxOffset; x++) {
      allYears.push(thisYear - x);
    }

    const yearList = allYears.map(x => {
      return <Select.Option key={x}>{x}</Select.Option>;
    });
    return (
      <div>
        <Select
          allowClear
          value={this.state.value}
          onChange={this.props.onChange}
        >
          {yearList}
        </Select>
      </div>
    );
  }
}
export { InputTrim, InputTrimTextArea, DropDownYear };
