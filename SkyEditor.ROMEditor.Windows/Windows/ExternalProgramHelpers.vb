﻿Namespace Windows
    Public Class ExternalProgramManager
        Implements IDisposable

        Public Sub New()
            ToDelete = New List(Of String)
        End Sub

        ''' <summary>
        ''' Tracks which files to delete on dispose
        ''' </summary>
        ''' <returns></returns>
        Private Property ToDelete As List(Of String)

        Private Function GetToolsDir() As String
            Dim path = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SkyEditor.ROMEditor")
            If Not IO.Directory.Exists(path) Then
                IO.Directory.CreateDirectory(path)
            End If
            Return path
        End Function
#Region "Paths"

        Public Function GetPPMDUnPXPath() As String
            Dim path = IO.Path.Combine(GetToolsDir, "ppmd_unpx.exe")
            If Not IO.File.Exists(path) Then
                IO.File.WriteAllBytes(path, My.Resources.ppmd_unpx)
                ToDelete.Add(path)
            End If
            Return path
        End Function

        Public Function GetPPMDPXCompPath() As String
            Dim path = IO.Path.Combine(GetToolsDir, "ppmd_pxcomp.exe")
            If Not IO.File.Exists(path) Then
                IO.File.WriteAllBytes(path, My.Resources.ppmd_pxcomp)
                ToDelete.Add(path)
            End If
            Return path
        End Function

        Public Function GetStatsUtilPath() As String
            Dim dataPath = IO.Path.Combine(GetToolsDir, "pmd2data.xml")
            If Not IO.File.Exists(dataPath) Then
                IO.File.WriteAllText(dataPath, My.Resources.pmd2data)
                ToDelete.Add(dataPath)
            End If

            Dim scriptDataPath = IO.Path.Combine(GetToolsDir, "pmd2scriptdata.xml")
            If Not IO.File.Exists(scriptDataPath) Then
                IO.File.WriteAllText(scriptDataPath, My.Resources.pmd2scriptdata)
                ToDelete.Add(scriptDataPath)
            End If

            Dim path = IO.Path.Combine(GetToolsDir, "ppmd_statsutil.exe")
            If Not IO.File.Exists(path) Then
                IO.File.WriteAllBytes(path, My.Resources.ppmd_statsutil)
                ToDelete.Add(path)
            End If
            Return path
        End Function

#End Region

#Region "Run"

        Public Async Function RunPPMDUnPX(arguments As String) As Task
            Await SkyEditor.Core.Windows.Processes.ConsoleApp.RunProgram(GetPPMDUnPXPath, arguments).ConfigureAwait(False)
        End Function

        Public Async Function RunPPMDPXComp(arguments As String) As Task
            Await SkyEditor.Core.Windows.Processes.ConsoleApp.RunProgramNoOutput(GetPPMDPXCompPath, arguments).ConfigureAwait(False)
        End Function

        Public Async Function RunPPMDStatsUtil(arguments As String) As Task
            Await SkyEditor.Core.Windows.Processes.ConsoleApp.RunProgramNoOutput(GetStatsUtilPath, arguments).ConfigureAwait(False)
        End Function

#End Region

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                    For Each item In ToDelete
                        If IO.File.Exists(item) Then
                            IO.File.Delete(item)
                        ElseIf IO.Directory.Exists(item) Then
                            IO.Directory.Delete(item, True)
                        End If
                    Next
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            ' TODO: uncomment the following line if Finalize() is overridden above.
            ' GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class

End Namespace
