import React from "react";
import { Table, Empty } from "antd";
import PropTypes from "prop-types";

class TableCustom extends React.PureComponent {
  render() {
    const { dataSource, keyword, onSearch } = this.props;

    let localeDefault = {
      emptyText: <Empty image={Empty.PRESENTED_IMAGE_SIMPLE} description="Không có dữ liệu" />,
    };
    let props = {
      scroll: { y: "calc(100vh - 350px)", x: 1000 },
      ...this.props,
      locale: {
        ...localeDefault,
        ...this.props.locale,
      },
    };

    if (typeof onSearch === "function") {
      const _dataSource = onSearch(dataSource ? dataSource : [], keyword);
      props = {
        ...this.props,
        dataSource: _dataSource,
      };
    }
    return <Table {...props} bordered />;
  }
}
TableCustom.propTypes = {
  onSearch: PropTypes.func,
  keyword: PropTypes.string,
};
TableCustom.defaultProps = {
  keyword: "",
};

export default TableCustom;
