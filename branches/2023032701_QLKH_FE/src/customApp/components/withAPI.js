import { padding } from "polished";
import React from "react";
import { Modal, Spin } from "antd";
import { getFilterData } from "../../helpers/utility";
import PropTypes from "prop-types";

// Take in a component as argument WrappedComponent
export const withAPI = (WrappedComponent) => {
  // And return another component
  class HOC extends React.PureComponent {
    timer = null;
    defaultValue;
    constructor(props) {
      super(props);
      this.state = {
        data: [],
        loading: false,
      };
      this.Id = makeElementID(5);
    }
    componentDidMount() {
      if (this.props.noInitData) {
        return;
      } else {
        // console.log(this.props.apiConfig);
        this.initData();
      }
    }
    initData = () => {
      if (this.state.data.length === 0 && !this.inited) {
        const { apiConfig } = this.props;
        const { api } = apiConfig;
        if (api) {
          this.getData();
        }
      }
    };
    getData = (filter = {}) => {
      const { apiConfig } = this.props;
      const { api, valueField, nameField, customPageSize } = apiConfig;

      api({ ...filter, ...this.props.filter, ...apiConfig.filter }).then((res) => {
        if (res.status !== 200 || !res.data) {
          Modal.error({
            title: "Lỗi",
            content: `${res.status} - ${res.statusText}`,
          });
          this.setState({ loading: false });
        } else {
          if (res.data.Status !== 1) {
            Modal.error({
              title: "Lỗi",
              content: res.data.Message,
            });
            this.setState({ loading: false });
          } else {
            let resData = res.data.Data;

            const data = resData.map((item) => {
              let newItem = Object.assign({}, item);
              newItem.text = item[nameField];
              newItem.value = item[valueField];
              if (newItem.value === this.props.value) {
                this.defaultValue = newItem;
                // console.log(newItem);
              }
              return newItem;
            });
            this.inited = true;
            this.setState({ data, loading: false });
          }
        }
      });
    };

    onSearch = (value) => {
      if (this.state.loading === false) {
        this.setState({ loading: true, data: [] });
      }
      if (this.timer) {
        clearTimeout(this.timer);
      }

      this.timer = setTimeout(() => {
        let onFilter = { value, property: "Keyword" };
        let filterData = getFilterData({}, onFilter, null);
        this.getData(filterData);
      }, 1000);
    };
    withSearchAPIRender = () => {
      const { data, loading } = this.state;
      return (
        <WrappedComponent
          notFoundContent={
            loading ? (
              <div className="text-center">
                <Spin size="small" />
              </div>
            ) : null
          }
          onScrol
          onFocus={this.initData}
          onSearch={this.onSearch}
          loading={this.state.loading}
          data={data}
          {...this.props}
          id={this.Id}
        ></WrappedComponent>
      );
    };
    render() {
      const { loading } = this.state;
      let data = [];
      if (this.inited) {
        data = this.state.data;
      } else {
        data = this.props.data;
      }

      if (this.props.useAPISearch) {
        return this.withSearchAPIRender();
      }
      return (
        <WrappedComponent
          notFoundContent={
            loading ? (
              <div className="text-center">
                <Spin size="small" />
              </div>
            ) : null
          }
          onFocus={this.initData}
          loading={this.state.loading}
          data={data}
          {...this.props}
          id={this.Id}
        ></WrappedComponent>
      );
    }
  }
  HOC.propTypes = {
    noInit: PropTypes.bool,
  };
  return HOC;
};
function makeElementID(length) {
  var result = "";
  var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
  var charactersLength = characters.length;
  for (var i = 0; i < length; i++) {
    result += characters.charAt(Math.floor(Math.random() * charactersLength));
  }
  return result;
}
