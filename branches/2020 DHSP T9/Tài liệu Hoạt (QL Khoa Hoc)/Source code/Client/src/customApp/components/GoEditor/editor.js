import React, { Component } from "react";
import ReactQuill from "react-quill";
import "react-quill/dist/quill.snow.css";
import "react-quill/dist/quill.core.css";
import QuillEditor from "./editor.style";
import { Spin } from "antd";

import { Input } from "antd";
const { TextArea } = Input;
export class Editor extends React.PureComponent {
  constructor(props) {
    super(props);
    // this.handleChange = this.handleChange.bind(this);
    this.state = { value: "" };
    this.timeout;
    this.quillModules = {
      toolbar: {
        container: [
          [{ header: [1, 2, false] }, { font: [] }],
          ["bold", "italic", "underline", "strike", "blockquote"],
          [{ list: "ordered" }, { list: "bullet" }, { indent: "-1" }, { indent: "+1" }],
          ["link", "image", "video"],
          ["clean"],
        ],
      },
    };
  }
  componentDidMount() {
    this.props.value ? this.initData() : null;
  }
  initData = () => {
    this.setState({ value: this.props.value });
  };
  handleChange = (value) => {
    if (this.timeout) clearTimeout(this.timeout);
    this.timeout = setTimeout(() => {
      this.props.onDone ? this.props.onDone(value) : null;
    }, 1500);
    this.setState({ value });
  };

  render() {
    const options = {
      theme: "snow",
      formats: Editor.formats,
      placeholder: "",
      value: this.state.value,
      onChange: this.handleChange,
      modules: this.quillModules,
    };
    return (
      <QuillEditor>
        <ReactQuill {...options} />
      </QuillEditor>
    );
  }
}
// Take in a component as argument WrappedComponent
export const withInputToText = (WrappedComponent) => {
  // And return another component
  class HOC extends React.Component {
    constructor(props) {
      super(props);
      this.state = {
        isFocused: false,
      };
      this.Editor = React.createRef();
    }

    toggle = () => {
      if (this.state.isFocused === false) {
        // console.log(this.Editor.current);
        // this.Editor.current.initData();
        this.setState({ isFocused: true });
      }
    };
    render() {
      const { isFocused } = this.state;
      // console.log(this.props.disabled);
      if (this.props.disabled) {
        return (
          <div className="withInputToText text-area-border" onClick={this.toggle} style={{ padding: "5px 12px" }}>
            <div style={{ minHeight: 80, cursor: "pointer" }} dangerouslySetInnerHTML={{ __html: this.props.value }}></div>
          </div>
        );
      }
      return (
        <div className="withInputToText text-area-border" onClick={this.toggle} style={{ padding: isFocused ? 0 : "5px 12px" }}>
          {isFocused ? <WrappedComponent ref={this.Editor} {...this.props} /> : <div style={{ minHeight: 80, cursor: "pointer" }} dangerouslySetInnerHTML={{ __html: this.props.value }}></div>}
        </div>
      );
    }
  }
  return HOC;
};
export default Editor = withInputToText(Editor);
