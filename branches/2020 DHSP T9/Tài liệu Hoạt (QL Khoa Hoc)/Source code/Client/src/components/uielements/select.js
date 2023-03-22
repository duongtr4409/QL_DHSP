import React from 'react';
import {Select, Empty} from 'antd';
import {getScrollParent} from '../../helpers/utility';

class CustomerSelect extends React.PureComponent {
  render() {
    const props = {
      getPopupContainer: (e) => this.props.getPopupContainer ? this.props.getPopupContainer : getScrollParent(e),
      ...this.props
    };
    return <Select
      {...props}
      notFoundContent={props.notFoundContent ? props.notFoundContent : <Empty description={"Không có dữ liệu"}/>}
      optionFilterProp={props.optionFilterProp !== undefined ? props.optionFilterProp : "children"}
      dropdownStyle={{
        maxHeight: 400,
        width: 'auto',
        overflow: 'auto',
        ...this.props.dropdownStyle
      }}
    />
  }
}

const Option = Select.Option;
const OptGroup = Select.OptGroup;

export default CustomerSelect;
export {Option, OptGroup};