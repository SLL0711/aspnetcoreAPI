using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Lib.Database
{
    /// <summary>
    /// 非线程安全 不支持多线程访问
    /// </summary>
    public class SqlHelper : IDisposable
    {
        private string _connStr;
        private SqlConnection _conn;
        private int _timeout = 30;//连接超时 默认-1表示使用系统默认

        public SqlHelper(string connectStr)
        {
            if (string.IsNullOrWhiteSpace(connectStr))
            {
                throw new ArgumentNullException(nameof(connectStr));
            }

            _connStr = connectStr;
        }

        public SqlHelper(string connectStr, int timeout)
            : this(connectStr)
        {
            if (timeout > 0)
            {
                this._timeout = timeout;
            }
        }

        /// <summary>
        /// 尝试打开数据库连接
        /// </summary>
        /// <returns></returns>
        private bool TryOpen()
        {
            bool dispose = false;
            try
            {
                if (_conn == null)
                {
                    _conn = new SqlConnection(_connStr);
                }

                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }

                return _conn.State == ConnectionState.Open;
            }
            catch (Exception ex)
            {
                dispose = true;
                return false;
            }
            finally
            {
                if (dispose)
                {
                    Dispose();
                }
            }
        }

        public DataTable GetDataTable(string sql)
        {
            return GetDataTable(sql, null);
        }

        public DataTable GetDataTable(string sql, params IDataParameter[] parameters)
        {
            try
            {
                if (!TryOpen())
                {
                    return null;
                }

                using (SqlCommand cmd = new SqlCommand(sql, _conn))
                {
                    cmd.CommandTimeout = _timeout;

                    if (parameters != null)
                    {
                        foreach (IDataParameter param in parameters)
                        {
                            cmd.Parameters.Add(param);
                        }
                    }

                    using (DataSet ds = new DataSet())
                    {
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        sda.Fill(ds);

                        if (ds.Tables != null && ds.Tables.Count > 0)
                        {
                            return ds.Tables[0];
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataRow GetDataRow(string sql, params IDataParameter[] parameters)
        {
            var dt = GetDataTable(sql, parameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0];
            }

            return null;
        }

        public DataRow GetDataRow(string sql)
        {
            return GetDataRow(sql, null);
        }

        public object ExecuteScalar(string sql, params IDataParameter[] parameters)
        {
            try
            {
                if (!TryOpen())
                {
                    return null;
                }

                using (SqlCommand cmd = new SqlCommand(sql, _conn))
                {
                    cmd.CommandTimeout = _timeout;

                    if (parameters != null)
                    {
                        foreach (IDataParameter param in parameters)
                        {
                            cmd.Parameters.Add(param);
                        }
                    }

                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public object ExecuteScalar(string sql)
        {
            return ExecuteScalar(sql, null);
        }

        public int ExecuteNonQuery(string sql, params IDataParameter[] parameters)
        {
            try
            {
                if (!TryOpen())
                {
                    return 0;
                }

                using (SqlCommand cmd = new SqlCommand(sql, _conn))
                {
                    cmd.CommandTimeout = _timeout;

                    if (parameters != null)
                    {
                        foreach (IDataParameter param in parameters)
                        {
                            cmd.Parameters.Add(param);
                        }
                    }

                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int ExecuteNonQuery(string sql)
        {
            return ExecuteNonQuery(sql, null);
        }

        public bool ExecTrans(string sql, params IDataParameter[] parameters)
        {
            try
            {
                if (!TryOpen())
                {
                    return false;
                }

                using (var trans = _conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(sql, _conn, trans))
                        {
                            cmd.CommandTimeout = _timeout;

                            if (parameters != null)
                            {
                                foreach (IDataParameter param in parameters)
                                {
                                    cmd.Parameters.Add(param);
                                }
                            }

                            cmd.ExecuteNonQuery();
                        }

                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();

                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ExecTrans(string sql)
        {
            return ExecTrans(sql, null);
        }

        public DataTable ExecProc(string procname, params IDataParameter[] parameters)
        {
            if (!TryOpen())
            {
                return null;
            }

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandTimeout = _timeout;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = procname;
                cmd.Connection = _conn;

                if (parameters != null)
                {
                    foreach (IDataParameter param in parameters)
                    {
                        //TODO：数据库存储过程中的Direction
                        // 检查未分配值的输出参数,将其分配以DBNull.Value.
                        if ((param.Direction == ParameterDirection.InputOutput || param.Direction == ParameterDirection.Input) &&
                            (param.Value == null))
                        {
                            param.Value = DBNull.Value;
                        }
                        cmd.Parameters.Add(param);
                    }
                }

                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(ds);

                return (ds != null && ds.Tables != null && ds.Tables.Count > 0) ? ds.Tables[0] : null;
            }
        }

        public DataTable ExecProc(string procname)
        {
            return ExecProc(procname);
        }

        public void Dispose()
        {
            //释放数据库连接资源
            if (_conn != null)
            {
                if (_conn.State == ConnectionState.Open)
                {
                    _conn.Close();
                }
            }
        }
    }
}
