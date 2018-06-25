import React from 'react'
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import Ink from 'react-ink';
import * as types from '../constants/Types.jsx'
import moment from 'moment'

class Jingle extends React.Component {
	constructor(props) {
		super(props);
		this.click = this.click.bind(this);

	}

	click(e) {
		e.preventDefault();
		this.props.onclick(this.props.filePath);
	}

	render() {
		return (
			<div className="jingle-item" onClick={this.click}>
				<div className="jingle-item-title">
					{this.props.name}
				</div>
				<div className="jingle-item-add">
					<i className="plus icon"></i>
				</div>

				<Ink />
			</div>)
	}
}

Jingle.propTypes = {
	filePath: PropTypes.string,
	name: PropTypes.string,
	
}

function mapStateToProps(state) {
	return {
	}
}

export default connect(mapStateToProps)(Jingle)
