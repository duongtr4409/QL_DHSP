import React, { PureComponent } from "react";

export default class DropDownWrap extends PureComponent {
  constructor(props) {
    super(props);
    const { allHeight, startIndex, endIndex } = props;

    this.state = {
      allHeight,
      startIndex,
      endIndex
    };
  }

  componentDidUpdate(prevProps) {
    if (this.props.allHeight !== prevProps.allHeight) {
      this.setState({ allHeight: this.props.allHeight });
    }
  }

  getItemStyle = i => {
    const { itemHeight } = this.props;
    return {
      position: "absolute",
      top: itemHeight * i ,
      height: itemHeight,
      width: "100%"
    };
  };
  componentDidMount(){
    const { menu } = this.props;
    const menuDiv = document.getElementById(menu.props.ariaId)
    // console.log(menuDiv)
    menuDiv.style.height = ''
  }
  reactList = (allHeight, startIndex, endIndex) =>{

    this.setState({ allHeight, startIndex, endIndex });
  }
  

  render() {
    const { menu } = this.props;
    const { startIndex, endIndex, allHeight } = this.state;
    // console.log(menu)
    // 截取 Select 下拉列表中需要显示的部分
    const cloneMenu = React.cloneElement(menu, {
      menuItems: menu.props.menuItems
        .slice(startIndex, endIndex)
        .map((item, i) => {
          const realIndex = (startIndex || 0) + Number(i);
          const style = this.getItemStyle(realIndex);
        
          // 未搜到数据提示高度使用默认高度
       
          return React.cloneElement(item, {
            style: { ...item.props.style, ...style }
          });
        }),
      dropdownMenuStyle: {
        ...menu.props.dropdownMenuStyle,
        height:  allHeight,
        maxHeight: allHeight ,
        overflow: "hidden"
      }
    });

    return cloneMenu

  }
}
